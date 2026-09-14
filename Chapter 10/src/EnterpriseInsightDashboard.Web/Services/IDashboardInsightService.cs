using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public interface IDashboardInsightService
{
    Task<DashboardInsight> BuildInsightAsync(
        CancellationToken cancellationToken);
}
