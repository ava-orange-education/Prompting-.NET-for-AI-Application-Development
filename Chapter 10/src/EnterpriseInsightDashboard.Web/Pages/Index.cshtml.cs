using EnterpriseInsightDashboard.Web.Models;
using EnterpriseInsightDashboard.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EnterpriseInsightDashboard.Web.Pages;

public sealed class IndexModel(
    IEnterpriseDataService dataService,
    IDashboardInsightService insightService) : PageModel
{
    public EnterpriseMetricSnapshot Snapshot { get; private set; } = default!;
    public DashboardInsight Insight { get; private set; } = default!;

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Snapshot = await dataService.GetCurrentSnapshotAsync(cancellationToken);
        Insight = await insightService.BuildInsightAsync(cancellationToken);
    }
}
