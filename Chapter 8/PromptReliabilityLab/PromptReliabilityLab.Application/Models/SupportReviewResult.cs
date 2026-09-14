namespace PromptReliabilityLab.Application.Models;

public sealed record SupportReviewResult(
    string Title,
    string Category,
    string Priority,
    string Summary,
    string RecommendedAction,
    bool UsedFallback,
    string PromptVersion,
    IReadOnlyList<string> ValidationWarnings);