namespace EnterpriseInsightDashboard.Web.Models;

public sealed record EnterpriseMetricSnapshot(
    DateTimeOffset CreatedAt,
    string Region,
    string ReportingPeriod,
    IReadOnlyList<DashboardKpi> Kpis,
    IReadOnlyList<string> OperationalNotes);
