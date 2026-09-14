using PromptReliabilityLab.AI.Prompts;
using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.AI.Parsing;

public static class ReviewFallback
{
    public static SupportReviewResult Create(string reason)
        => new(
            Title: "Support Review Required",
            Category: "Other",
            Priority: "Medium",
            Summary: "The request could not be analyzed automatically in a reliable way.",
            RecommendedAction: $"Review the message manually. Reason: {reason}",
            UsedFallback: true,
            PromptVersion: SupportReviewPrompts.Version,
            ValidationWarnings: [reason]);
}