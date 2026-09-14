namespace PromptReliabilityLab.Application.Models;

public sealed record ReviewClassification(
    string Category,
    string Priority,
    bool NeedsHumanReview);