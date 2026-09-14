namespace EnterpriseInsightDashboard.Web.Models;

public sealed record DashboardKpi(
    string Name,
    decimal Value,
    string Unit,
    decimal PreviousValue,
    string Trend,
    string BusinessArea);
