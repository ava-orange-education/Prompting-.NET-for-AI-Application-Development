using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.Application.Contracts;

public interface IReviewContextProvider
{
    Task<ReviewContext> BuildAsync(
        ReviewWorkflowRequest request,
        CancellationToken cancellationToken);
}