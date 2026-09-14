using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public interface IDashboardTelemetryWriter
{
    void Write(DashboardRunTelemetry telemetry);
}
