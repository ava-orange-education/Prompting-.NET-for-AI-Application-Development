using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.Application.Contracts;

public interface IReviewGenerator
{
    Task<SupportReviewResult> GenerateAsync(
        ReviewContext context,
        ReviewClassification classification,
        CancellationToken cancellationToken);
}