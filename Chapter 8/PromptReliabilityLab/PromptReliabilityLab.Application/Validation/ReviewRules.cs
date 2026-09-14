namespace PromptReliabilityLab.Application.Validation;

public static class ReviewRules
{
    public static readonly string[] AllowedCategories =
    [
        "Bug",
        "Billing",
        "Access",
        "Performance",
        "Security",
        "Other"
    ];

    public static readonly string[] AllowedPriorities =
    [
        "Low",
        "Medium",
        "High"
    ];
}