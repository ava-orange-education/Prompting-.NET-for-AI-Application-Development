using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public interface IEnterpriseDataService
{
    Task<EnterpriseMetricSnapshot> GetCurrentSnapshotAsync(
        CancellationToken cancellationToken);
}
