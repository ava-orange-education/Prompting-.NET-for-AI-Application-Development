namespace EnterpriseInsightDashboard.Web.Models;

public sealed class PromptBudgetOptions
{
    public int MaxInputCharacters { get; init; } = 6_000;
    public int MaxOutputTokens { get; init; } = 700;
    public int TimeoutSeconds { get; init; } = 12;
}
