namespace PromptReliabilityLab.Application.Budgets;

public static class PromptBudgets
{
    public static PromptBudget SupportReview => new(
        MaxInputCharacters: 4_000,
        MaxOutputTokens: 600,
        Timeout: TimeSpan.FromSeconds(12));
}