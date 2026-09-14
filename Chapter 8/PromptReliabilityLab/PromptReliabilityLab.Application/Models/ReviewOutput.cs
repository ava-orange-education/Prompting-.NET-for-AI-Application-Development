namespace PromptReliabilityLab.Application.Models;

public sealed record ReviewOutput(
    string Title,
    string Category,
    string Priority,
    string Summary,
    string RecommendedAction);