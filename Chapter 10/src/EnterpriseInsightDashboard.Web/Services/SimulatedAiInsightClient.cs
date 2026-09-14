using System.Diagnostics;
using System.Text.Json;
using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public sealed class SimulatedAiInsightClient : IAiInsightClient
{
    public Task<InsightGenerationResponse> GenerateAsync(
        InsightGenerationRequest request,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = new
        {
            executiveSummary = "The dashboard shows a positive revenue movement, but SLA compliance and inventory risk need attention before they affect customer experience.",
            riskLevel = "Medium",
            narratives = request.Snapshot.Kpis.Select(kpi => new
            {
                kpiName = kpi.Name,
                narrative = BuildNarrative(kpi),
                impact = BuildImpact(kpi),
                suggestedAction = BuildAction(kpi)
            }),
            recommendedActions = new[]
            {
                "Review inventory items with rising risk before the next replenishment cycle.",
                "Analyze SLA delays by region and escalate the most repeated blockers.",
                "Keep the renewal campaign active while monitoring support capacity."
            }
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        stopwatch.Stop();
        return Task.FromResult(new InsightGenerationResponse(
            json,
            Provider: "Simulated",
            DurationMs: stopwatch.ElapsedMilliseconds));
    }

    private static string BuildNarrative(DashboardKpi kpi)
        => kpi.Trend switch
        {
            "Up" => $"{kpi.Name} increased compared with the previous period in the {kpi.BusinessArea} area.",
            "Down" => $"{kpi.Name} decreased compared with the previous period in the {kpi.BusinessArea} area.",
            _ => $"{kpi.Name} remained stable in the {kpi.BusinessArea} area."
        };

    private static string BuildImpact(DashboardKpi kpi)
        => kpi.Name.Contains("Risk", StringComparison.OrdinalIgnoreCase) && kpi.Trend == "Up"
            ? "The increase may create operational pressure if it continues."
            : "The movement should be tracked with the next dashboard refresh.";

    private static string BuildAction(DashboardKpi kpi)
        => kpi.Name.Contains("SLA", StringComparison.OrdinalIgnoreCase)
            ? "Review delayed cases and assign an owner for the most repeated causes."
            : $"Validate the {kpi.BusinessArea} action plan for this KPI.";
}
