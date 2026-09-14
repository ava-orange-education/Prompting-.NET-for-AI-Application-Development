namespace EnterpriseInsightDashboard.Web.Models;

public sealed record DashboardRunTelemetry(
    string FeatureName,
    string PromptVersion,
    string Provider,
    int InputCharacters,
    int OutputCharacters,
    long DurationMs,
    bool UsedFallback,
    string? ValidationFailureReason);
