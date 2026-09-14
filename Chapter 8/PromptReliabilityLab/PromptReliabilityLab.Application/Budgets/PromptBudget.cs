namespace PromptReliabilityLab.Application.Budgets;

public sealed record PromptBudget(
    int MaxInputCharacters,
    int MaxOutputTokens,
    TimeSpan Timeout);