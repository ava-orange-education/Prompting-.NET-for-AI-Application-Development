using PromptReliabilityLab.Application.Budgets;

namespace PromptReliabilityLab.Tests.Budgets;

public sealed class PromptInputLimiterTests
{
    [Fact]
    public void Cap_ReturnsEmpty_WhenInputIsEmpty()
    {
        var result = PromptInputLimiter.Cap(" ", 10);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Cap_ReturnsOriginalValue_WhenInputIsInsideBudget()
    {
        var result = PromptInputLimiter.Cap("Valid input", 50);

        Assert.Equal("Valid input", result);
    }

    [Fact]
    public void Cap_TrimsInput_WhenInputExceedsBudget()
    {
        var result = PromptInputLimiter.Cap("1234567890", 5);

        Assert.Equal("12345", result);
    }
}