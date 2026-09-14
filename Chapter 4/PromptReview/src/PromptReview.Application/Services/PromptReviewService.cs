using PromptReview.Application.Abstractions;
using PromptReview.Contracts.Models;

namespace PromptReview.Application.Services;

public sealed class PromptReviewService
{
    private readonly IAiModelClient _aiModelClient;

    public PromptReviewService(IAiModelClient aiModelClient)
    {
        _aiModelClient = aiModelClient;
    }

    public async Task<PromptReviewResponse> ReviewAsync(
        PromptReviewRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            throw new ArgumentException("The text to review cannot be empty.");
        }

        return await _aiModelClient.ReviewTextAsync(request, cancellationToken);
    }
}