using PromptReliabilityLab.AI.Parsing;

namespace PromptReliabilityLab.Tests.Parsing;

public sealed class ReviewResultParserTests
{
    [Fact]
    public void TryParse_ReturnsTrue_WhenJsonIsValid()
    {
        var json = """
        {
          "title": "Orders Page Is Slow",
          "category": "Performance",
          "priority": "High",
          "summary": "The orders page becomes slow after applying filters.",
          "recommendedAction": "Review query performance and collect logs."
        }
        """;

        var parsed = ReviewResultParser.TryParse(
            json,
            "support-review-v1",
            out var result,
            out var reason);

        Assert.True(parsed);
        Assert.Equal("Performance", result.Category);
        Assert.Equal("High", result.Priority);
        Assert.Equal(string.Empty, reason);
    }

    [Fact]
    public void TryParse_ReturnsFalse_WhenCategoryIsInvalid()
    {
        var json = """
        {
          "title": "Orders Page Is Slow",
          "category": "UnknownCategory",
          "priority": "High",
          "summary": "The orders page becomes slow after applying filters.",
          "recommendedAction": "Review query performance and collect logs."
        }
        """;

        var parsed = ReviewResultParser.TryParse(
            json,
            "support-review-v1",
            out _,
            out var reason);

        Assert.False(parsed);
        Assert.Contains("Category is not allowed", reason);
    }
}