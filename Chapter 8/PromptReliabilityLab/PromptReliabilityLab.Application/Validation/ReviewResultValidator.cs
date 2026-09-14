using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.Application.Validation;

public static class ReviewResultValidator
{
    public static bool IsValid(
        SupportReviewResult result,
        out IReadOnlyList<string> warnings)
    {
        var issues = new List<string>();

        if (string.IsNullOrWhiteSpace(result.Title))
            issues.Add("Title is required.");

        if (string.IsNullOrWhiteSpace(result.Summary))
            issues.Add("Summary is required.");

        if (string.IsNullOrWhiteSpace(result.RecommendedAction))
            issues.Add("Recommended action is required.");

        if (!ReviewRules.AllowedCategories.Contains(result.Category))
            issues.Add($"Category is not allowed: {result.Category}");

        if (!ReviewRules.AllowedPriorities.Contains(result.Priority))
            issues.Add($"Priority is not allowed: {result.Priority}");

        warnings = issues;

        return issues.Count == 0;
    }
}