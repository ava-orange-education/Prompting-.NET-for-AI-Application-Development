using EnterpriseInsightDashboard.Web.Models;
using Microsoft.Extensions.Options;

namespace EnterpriseInsightDashboard.Web.Services;

public sealed class DashboardInsightService(
    IEnterpriseDataService dataService,
    IPromptBuilder promptBuilder,
    IAiInsightClient aiClient,
    IGuardrailService guardrailService,
    IStructuredInsightValidator validator,
    IDashboardTelemetryWriter telemetryWriter,
    IOptions<PromptBudgetOptions> budgetOptions) : IDashboardInsightService
{
    private readonly PromptBudgetOptions _budget = budgetOptions.Value;

    public async Task<DashboardInsight> BuildInsightAsync(
        CancellationToken cancellationToken)
    {
        var snapshot = await dataService.GetCurrentSnapshotAsync(cancellationToken);
        var prompt = promptBuilder.Build(snapshot);
        var guardrail = guardrailService.ValidatePrompt(prompt, _budget);

        if (!guardrail.IsAllowed)
        {
            var telemetry = CreateTelemetry(
                provider: "Guardrail",
                prompt,
                outputCharacters: 0,
                durationMs: 0,
                usedFallback: true,
                guardrail.Reason);

            telemetryWriter.Write(telemetry);
            return CreateFallback(guardrail.Reason ?? "The request was blocked by a guardrail.", telemetry);
        }

        using var timeout = new CancellationTokenSource(
            TimeSpan.FromSeconds(_budget.TimeoutSeconds));
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            timeout.Token);

        try
        {
            var response = await aiClient.GenerateAsync(
                new InsightGenerationRequest(
                    snapshot,
                    prompt,
                    promptBuilder.Version,
                    _budget.MaxOutputTokens),
                linked.Token);

            var telemetry = CreateTelemetry(
                response.Provider,
                prompt,
                response.RawContent.Length,
                response.DurationMs,
                usedFallback: false,
                validationFailureReason: null);

            if (validator.TryParse(
                response.RawContent,
                promptBuilder.Version,
                telemetry,
                out var insight,
                out var reason))
            {
                telemetryWriter.Write(telemetry);
                return insight;
            }

            var fallbackTelemetry = telemetry with
            {
                UsedFallback = true,
                ValidationFailureReason = reason
            };

            telemetryWriter.Write(fallbackTelemetry);
            return CreateFallback(reason, fallbackTelemetry);
        }
        catch (OperationCanceledException)
        {
            var telemetry = CreateTelemetry(
                provider: "Timeout",
                prompt,
                outputCharacters: 0,
                durationMs: _budget.TimeoutSeconds * 1000,
                usedFallback: true,
                validationFailureReason: "The AI request exceeded the configured timeout.");

            telemetryWriter.Write(telemetry);
            return CreateFallback("The AI request exceeded the configured timeout.", telemetry);
        }
    }

    private DashboardRunTelemetry CreateTelemetry(
        string provider,
        string prompt,
        int outputCharacters,
        long durationMs,
        bool usedFallback,
        string? validationFailureReason)
        => new(
            FeatureName: "EnterpriseDashboard",
            PromptVersion: promptBuilder.Version,
            Provider: provider,
            InputCharacters: prompt.Length,
            OutputCharacters: outputCharacters,
            DurationMs: durationMs,
            UsedFallback: usedFallback,
            ValidationFailureReason: validationFailureReason);

    private static DashboardInsight CreateFallback(
        string reason,
        DashboardRunTelemetry telemetry)
        => new(
            ExecutiveSummary: "The dashboard could not generate an AI-assisted narrative automatically.",
            RiskLevel: "Medium",
            Narratives:
            [
                new(
                    "Manual Review",
                    "The enterprise metrics are available, but the AI narrative was replaced by a fallback response.",
                    "The dashboard remains usable, but the business explanation should be reviewed manually.",
                    $"Review the dashboard data manually. Reason: {reason}")
            ],
            RecommendedActions:
            [
                "Validate the latest metrics with the business owner.",
                "Review the AI provider configuration and telemetry details."
            ],
            UsedFallback: true,
            PromptVersion: telemetry.PromptVersion,
            Telemetry: telemetry);
}
