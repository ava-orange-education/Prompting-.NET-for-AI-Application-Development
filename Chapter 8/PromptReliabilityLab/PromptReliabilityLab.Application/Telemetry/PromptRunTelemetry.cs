namespace PromptReliabilityLab.Application.Telemetry;

public sealed record PromptRunTelemetry(
    string FeatureName,
    string PromptVersion,
    int InputCharacters,
    long DurationMs,
    bool UsedFallback,
    bool WasTrimmed);