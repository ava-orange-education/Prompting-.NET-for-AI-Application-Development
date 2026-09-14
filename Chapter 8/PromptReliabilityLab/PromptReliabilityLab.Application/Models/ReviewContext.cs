namespace PromptReliabilityLab.Application.Models;

public sealed record ReviewContext(
    string NormalizedInput,
    string ProductArea,
    string CustomerTier,
    int InputCharacters,
    bool WasTrimmed);