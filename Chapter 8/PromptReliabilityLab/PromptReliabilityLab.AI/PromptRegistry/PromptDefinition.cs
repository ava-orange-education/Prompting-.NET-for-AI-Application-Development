namespace PromptReliabilityLab.AI.PromptRegistry;

public sealed record PromptDefinition(
    string Name,
    string Version,
    string Description,
    DateTimeOffset CreatedAt);