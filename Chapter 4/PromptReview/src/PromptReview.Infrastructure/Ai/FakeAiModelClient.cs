using PromptReview.Application.Abstractions;
using PromptReview.Contracts.Models;

namespace PromptReview.Infrastructure.Ai;

public sealed class FakeAiModelClient : IAiModelClient
{
    public async Task<PromptReviewResponse> ReviewTextAsync(
        PromptReviewRequest request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);

        return new PromptReviewResponse
        {
            Summary = "The submitted text was reviewed by the sample AI client.",
            Sentiment = DetectSentiment(request.Text),
            SuggestedActions =
            [
                "Validate the response contract",
                "Review the orchestration flow",
                "Replace the fake client with a real model provider when needed"
            ]
        };
    }

    private static string DetectSentiment(string text)
    {
        if (text.Contains("error", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("problem", StringComparison.OrdinalIgnoreCase))
        {
            return "Negative";
        }

        return "Neutral";
    }
}