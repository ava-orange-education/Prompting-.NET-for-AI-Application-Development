using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.Application.Contracts;

public interface IReviewClassifier
{
    Task<ReviewClassification> ClassifyAsync(
        ReviewContext context,
        CancellationToken cancellationToken);
}