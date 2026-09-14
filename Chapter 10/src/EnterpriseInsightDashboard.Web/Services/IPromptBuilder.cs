using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public interface IPromptBuilder
{
    string Version { get; }

    string Build(EnterpriseMetricSnapshot snapshot);
}
