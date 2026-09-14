using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public interface IAiInsightClient
{
    Task<InsightGenerationResponse> GenerateAsync(
        InsightGenerationRequest request,
        CancellationToken cancellationToken);
}
