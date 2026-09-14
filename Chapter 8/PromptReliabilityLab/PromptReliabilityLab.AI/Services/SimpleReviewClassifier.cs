using PromptReliabilityLab.Application.Contracts;
using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.AI.Services;

public sealed class SimpleReviewClassifier : IReviewClassifier
{
    public Task<ReviewClassification> ClassifyAsync(
        ReviewContext context,
        CancellationToken cancellationToken)
    {
        var input = context.NormalizedInput.ToLowerInvariant();

        var category = "Other";
        var priority = "Medium";
        var needsHumanReview = false;

        if (input.Contains("password") || input.Contains("login") || input.Contains("access"))
        {
            category = "Access";
        }
        else if (input.Contains("slow") || input.Contains("freezes") || input.Contains("timeout"))
        {
            category = "Performance";
        }
        else if (input.Contains("invoice") || input.Contains("payment") || input.Contains("billing"))
        {
            category = "Billing";
        }
        else if (input.Contains("security") || input.Contains("breach") || input.Contains("token"))
        {
            category = "Security";
            priority = "High";
            needsHumanReview = true;
        }
        else if (input.Contains("error") || input.Contains("bug") || input.Contains("crash"))
        {
            category = "Bug";
        }

        if (input.Contains("production") ||
            input.Contains("blocked") ||
            input.Contains("urgent"))
        {
            priority = "High";
            needsHumanReview = true;
        }

        var result = new ReviewClassification(
            Category: category,
            Priority: priority,
            NeedsHumanReview: needsHumanReview);

        return Task.FromResult(result);
    }
}