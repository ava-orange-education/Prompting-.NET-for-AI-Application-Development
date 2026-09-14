using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.AI.Prompts;

public static class SupportReviewPromptBuilder
{
    public static string BuildUserMessage(
        ReviewContext context,
        ReviewClassification classification)
        => $$"""
        Return JSON with this exact shape:
        {
          "title": "short title",
          "category": "Bug | Billing | Access | Performance | Security | Other",
          "priority": "Low | Medium | High",
          "summary": "two short sentences maximum",
          "recommendedAction": "one practical next step"
        }

        ProductArea: {{context.ProductArea}}
        CustomerTier: {{context.CustomerTier}}

        InitialCategory: {{classification.Category}}
        InitialPriority: {{classification.Priority}}
        NeedsHumanReview: {{classification.NeedsHumanReview}}

        SupportMessage:
        {{context.NormalizedInput}}
        """;
}