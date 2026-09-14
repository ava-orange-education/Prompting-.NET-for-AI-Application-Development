namespace PromptReview.Contracts.Models;

public sealed record PromptReviewResponse
{
    public string Summary { get; init; } = string.Empty;

    public string Sentiment { get; init; } = string.Empty;

    public IReadOnlyCollection<string> SuggestedActions { get; init; } = [];
}