using System.Text.Json;
using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public sealed class StructuredInsightValidator : IStructuredInsightValidator
{
    private static readonly HashSet<string> AllowedRiskLevels =
        new(StringComparer.OrdinalIgnoreCase) { "Low", "Medium", "High" };

    public bool TryParse(
        string rawContent,
        string promptVersion,
        DashboardRunTelemetry telemetry,
        out DashboardInsight insight,
        out string reason)
    {
        insight = default!;
        reason = string.Empty;

        try
        {
            var output = JsonSerializer.Deserialize<DashboardInsightCandidate>(
                rawContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (output is null ||
                string.IsNullOrWhiteSpace(output.ExecutiveSummary) ||
                string.IsNullOrWhiteSpace(output.RiskLevel) ||
                !AllowedRiskLevels.Contains(output.RiskLevel) ||
                output.Narratives is null ||
                output.Narratives.Count == 0)
            {
                reason = "The response did not match the dashboard contract.";
                return false;
            }

            insight = new DashboardInsight(
                output.ExecutiveSummary,
                output.RiskLevel,
                output.Narratives,
                output.RecommendedActions ?? [],
                UsedFallback: false,
                promptVersion,
                telemetry);

            return true;
        }
        catch (Exception ex)
        {
            reason = ex.Message;
            return false;
        }
    }

    private sealed record DashboardInsightCandidate(
        string ExecutiveSummary,
        string RiskLevel,
        IReadOnlyList<KpiNarrative> Narratives,
        IReadOnlyList<string>? RecommendedActions);
}
