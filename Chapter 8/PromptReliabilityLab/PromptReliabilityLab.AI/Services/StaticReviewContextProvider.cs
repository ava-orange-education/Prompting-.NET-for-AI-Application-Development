using PromptReliabilityLab.Application.Budgets;
using PromptReliabilityLab.Application.Contracts;
using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.AI.Services;

public sealed class StaticReviewContextProvider : IReviewContextProvider
{
    public Task<ReviewContext> BuildAsync(
        ReviewWorkflowRequest request,
        CancellationToken cancellationToken)
    {
        var budget = PromptBudgets.SupportReview;
        var cappedInput = PromptInputLimiter.Cap(
            request.Content,
            budget.MaxInputCharacters);

        var context = new ReviewContext(
            NormalizedInput: cappedInput,
            ProductArea: string.IsNullOrWhiteSpace(request.ProductArea)
                ? "Not provided"
                : request.ProductArea.Trim(),
            CustomerTier: string.IsNullOrWhiteSpace(request.CustomerTier)
                ? "Not provided"
                : request.CustomerTier.Trim(),
            InputCharacters: cappedInput.Length,
            WasTrimmed: PromptInputLimiter.WasTrimmed(
                request.Content,
                cappedInput));

        return Task.FromResult(context);
    }
}