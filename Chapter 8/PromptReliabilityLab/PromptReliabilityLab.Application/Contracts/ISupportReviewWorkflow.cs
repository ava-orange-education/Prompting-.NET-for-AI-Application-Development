using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.Application.Contracts;

public interface ISupportReviewWorkflow
{
    Task<SupportReviewResult> RunAsync(
        ReviewWorkflowRequest request,
        CancellationToken cancellationToken);
}