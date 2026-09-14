using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public sealed class FakeEnterpriseDataService : IEnterpriseDataService
{
    public Task<EnterpriseMetricSnapshot> GetCurrentSnapshotAsync(
        CancellationToken cancellationToken)
    {
        var snapshot = new EnterpriseMetricSnapshot(
            CreatedAt: DateTimeOffset.UtcNow,
            Region: "North America",
            ReportingPeriod: "Current Quarter",
            Kpis:
            [
                new("Revenue", 1_245_000, "USD", 1_120_000, "Up", "Sales"),
                new("Open Orders", 342, "Orders", 390, "Down", "Operations"),
                new("SLA Compliance", 94.6m, "%", 96.2m, "Down", "Customer Success"),
                new("Inventory Risk", 18, "Items", 11, "Up", "Supply Chain"),
                new("Support Backlog", 76, "Tickets", 91, "Down", "Support")
            ],
            OperationalNotes:
            [
                "Revenue increased after the renewal campaign.",
                "SLA compliance dropped because of delayed escalations in two regions.",
                "Inventory risk increased in high-demand replacement parts.",
                "Support backlog improved after weekend triage."
            ]);

        return Task.FromResult(snapshot);
    }
}
