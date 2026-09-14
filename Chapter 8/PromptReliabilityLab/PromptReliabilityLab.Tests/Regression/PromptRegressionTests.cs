using PromptReliabilityLab.AI.Prompts;
using PromptReliabilityLab.Application.Models;

namespace PromptReliabilityLab.Tests.Regression;

public sealed class PromptRegressionTests
{
    [Fact]
    public void SupportReviewPrompt_KeepsRequiredJsonFields()
    {
        var context = new ReviewContext(
            NormalizedInput: "The app freezes when opening the orders page.",
            ProductArea: "Orders",
            CustomerTier: "Business",
            InputCharacters: 52,
            WasTrimmed: false);

        var classification = new ReviewClassification(
            Category: "Performance",
            Priority: "High",
            NeedsHumanReview: true);

        var prompt = SupportReviewPromptBuilder.BuildUserMessage(
            context,
            classification);

        Assert.Contains("\"title\"", prompt);
        Assert.Contains("\"category\"", prompt);
        Assert.Contains("\"priority\"", prompt);
        Assert.Contains("\"summary\"", prompt);
        Assert.Contains("\"recommendedAction\"", prompt);
    }

    [Fact]
    public void SupportReviewPrompt_HasStableVersion()
    {
        Assert.Equal("support-review-v1", SupportReviewPrompts.Version);
    }
}