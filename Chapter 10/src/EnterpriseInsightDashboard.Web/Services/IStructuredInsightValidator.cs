using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public interface IStructuredInsightValidator
{
    bool TryParse(
        string rawContent,
        string promptVersion,
        DashboardRunTelemetry telemetry,
        out DashboardInsight insight,
        out string reason);
}
