namespace EnterpriseInsightDashboard.Web.Models;

public sealed record InsightGenerationRequest(
    EnterpriseMetricSnapshot Snapshot,
    string Prompt,
    string PromptVersion,
    int MaxOutputTokens);

public sealed record InsightGenerationResponse(
    string RawContent,
    string Provider,
    long DurationMs);

public sealed record GuardrailDecision(
    bool IsAllowed,
    string? Reason);
