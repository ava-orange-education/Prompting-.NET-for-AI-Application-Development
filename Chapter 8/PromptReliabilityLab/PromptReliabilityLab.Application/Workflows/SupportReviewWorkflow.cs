using Microsoft.Extensions.Logging;
using PromptReliabilityLab.Application.Contracts;
using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.Application.Workflows;

public sealed class SupportReviewWorkflow : ISupportReviewWorkflow
{
    private readonly IReviewContextProvider _contextProvider;
    private readonly IReviewClassifier _classifier;
    private readonly IReviewGenerator _generator;
    private readonly ILogger<SupportReviewWorkflow> _logger;

    public SupportReviewWorkflow(
        IReviewContextProvider contextProvider,
        IReviewClassifier classifier,
        IReviewGenerator generator,
        ILogger<SupportReviewWorkflow> logger)
    {
        _contextProvider = contextProvider;
        _classifier = classifier;
        _generator = generator;
        _logger = logger;
    }

    public async Task<SupportReviewResult> RunAsync(
        ReviewWorkflowRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return new SupportReviewResult(
                Title: "Support Review Required",
                Category: "Other",
                Priority: "Medium",
                Summary: "The request did not include enough content to analyze.",
                RecommendedAction: "Ask the user to provide more details before continuing.",
                UsedFallback: true,
                PromptVersion: "support-review-v1",
                ValidationWarnings: ["Content is required."]);
        }

        var context = await _contextProvider.BuildAsync(
            request,
            cancellationToken);

        var classification = await _classifier.ClassifyAsync(
            context,
            cancellationToken);

        var result = await _generator.GenerateAsync(
            context,
            classification,
            cancellationToken);

        _logger.LogInformation(
            "Support review completed. PromptVersion: {PromptVersion}, UsedFallback: {UsedFallback}, InputCharacters: {InputCharacters}, WasTrimmed: {WasTrimmed}",
            result.PromptVersion,
            result.UsedFallback,
            context.InputCharacters,
            context.WasTrimmed);

        return result;
    }
}