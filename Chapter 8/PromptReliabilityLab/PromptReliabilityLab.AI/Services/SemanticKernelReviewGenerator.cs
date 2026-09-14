using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using PromptReliabilityLab.AI.Parsing;
using PromptReliabilityLab.AI.Prompts;
using PromptReliabilityLab.Application.Budgets;
using PromptReliabilityLab.Application.Contracts;
using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.AI.Services;

public sealed class SemanticKernelReviewGenerator : IReviewGenerator
{
    private readonly Kernel _kernel;
    private readonly ILogger<SemanticKernelReviewGenerator> _logger;

    public SemanticKernelReviewGenerator(
        Kernel kernel,
        ILogger<SemanticKernelReviewGenerator> logger)
    {
        _kernel = kernel;
        _logger = logger;
    }

    public async Task<SupportReviewResult> GenerateAsync(
        ReviewContext context,
        ReviewClassification classification,
        CancellationToken cancellationToken)
    {
        try
        {
            using var timeoutCts = new CancellationTokenSource(
                PromptBudgets.SupportReview.Timeout);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                timeoutCts.Token);

            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();

            history.AddSystemMessage(SupportReviewPrompts.SystemMessage);
            history.AddUserMessage(
                SupportReviewPromptBuilder.BuildUserMessage(
                    context,
                    classification));

            var response = await chat.GetChatMessageContentAsync(
                history,
                cancellationToken: linkedCts.Token);

            var text = response.Content ?? string.Empty;

            if (ReviewResultParser.TryParse(
                text,
                SupportReviewPrompts.Version,
                out var result,
                out var reason))
            {
                return result;
            }

            _logger.LogWarning(
                "Invalid support review output. Reason: {Reason}",
                reason);

            return ReviewFallback.Create(reason);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("The AI request timed out.");

            return ReviewFallback.Create(
                "The AI request timed out.");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "The AI request failed.");

            return ReviewFallback.Create(
                $"The AI request failed: {ex.Message}");
        }
    }
}