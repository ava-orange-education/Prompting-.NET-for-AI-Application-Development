using System.Text;
using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public sealed class DashboardPromptBuilder : IPromptBuilder
{
    public string Version => "enterprise-dashboard-v1";

    public string Build(EnterpriseMetricSnapshot snapshot)
    {
        var builder = new StringBuilder();

        builder.AppendLine("You are an assistant inside an enterprise dashboard.");
        builder.AppendLine("Create a concise business narrative from the KPI snapshot.");
        builder.AppendLine("Return only valid JSON with this shape:");
        builder.AppendLine("{");
        builder.AppendLine("  \"executiveSummary\": \"string\",");
        builder.AppendLine("  \"riskLevel\": \"Low|Medium|High\",");
        builder.AppendLine("  \"narratives\": [");
        builder.AppendLine("    { \"kpiName\": \"string\", \"narrative\": \"string\", \"impact\": \"string\", \"suggestedAction\": \"string\" }");
        builder.AppendLine("  ],");
        builder.AppendLine("  \"recommendedActions\": [\"string\"]");
        builder.AppendLine("}");
        builder.AppendLine("Do not include markdown. Do not invent data not present in the snapshot.");
        builder.AppendLine();
        builder.AppendLine($"Region: {snapshot.Region}");
        builder.AppendLine($"ReportingPeriod: {snapshot.ReportingPeriod}");
        builder.AppendLine("KPIs:");

        foreach (var kpi in snapshot.Kpis)
        {
            builder.AppendLine(
                $"- {kpi.Name}: {kpi.Value} {kpi.Unit}; Previous: {kpi.PreviousValue} {kpi.Unit}; Trend: {kpi.Trend}; Area: {kpi.BusinessArea}");
        }

        builder.AppendLine("OperationalNotes:");

        foreach (var note in snapshot.OperationalNotes)
        {
            builder.AppendLine($"- {note}");
        }

        return builder.ToString();
    }
}
