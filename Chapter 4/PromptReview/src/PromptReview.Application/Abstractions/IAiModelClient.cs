using PromptReview.Contracts.Models;

namespace PromptReview.Application.Abstractions;

public interface IAiModelClient
{
    Task<PromptReviewResponse> ReviewTextAsync(
        PromptReviewRequest request,
        CancellationToken cancellationToken);
}