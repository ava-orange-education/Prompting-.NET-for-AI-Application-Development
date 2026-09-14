namespace PromptReliabilityLab.Application.Models;

public sealed record ReviewWorkflowRequest(
    string Content,
    string? ProductArea = null,
    string? CustomerTier = null);