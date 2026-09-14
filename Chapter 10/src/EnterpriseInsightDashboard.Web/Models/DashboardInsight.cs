namespace EnterpriseInsightDashboard.Web.Models;

public sealed record DashboardInsight(
    string ExecutiveSummary,
    string RiskLevel,
    IReadOnlyList<KpiNarrative> Narratives,
    IReadOnlyList<string> RecommendedActions,
    bool UsedFallback,
    string PromptVersion,
    DashboardRunTelemetry Telemetry);

public sealed record KpiNarrative(
    string KpiName,
    string Narrative,
    string Impact,
    string SuggestedAction);
