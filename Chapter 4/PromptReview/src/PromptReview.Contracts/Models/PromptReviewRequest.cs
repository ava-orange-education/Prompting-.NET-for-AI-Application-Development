namespace PromptReview.Contracts.Models;

public sealed record PromptReviewRequest
{
    public string Text { get; init; } = string.Empty;
}