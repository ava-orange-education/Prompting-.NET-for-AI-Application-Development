namespace EnterpriseInsightDashboard.Web.Models;

public sealed class AzureOpenAiOptions
{
    public string Endpoint { get; init; } = string.Empty;
    public string DeploymentName { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public string ApiVersion { get; init; } = "2025-01-01-preview";
}
