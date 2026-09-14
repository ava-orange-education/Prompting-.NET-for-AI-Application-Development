using PromptReliabilityLab.AI.Prompts;

namespace PromptReliabilityLab.AI.PromptRegistry;

public sealed class InMemoryPromptRegistry
{
    private readonly IReadOnlyList<PromptDefinition> _prompts =
    [
        new PromptDefinition(
            Name: "Support Review",
            Version: SupportReviewPrompts.Version,
            Description: "Analyzes support messages and returns structured JSON.",
            CreatedAt: new DateTimeOffset(2026, 5, 17, 0, 0, 0, TimeSpan.Zero))
    ];

    public PromptDefinition GetCurrent(string name)
    {
        return _prompts.First(prompt =>
            prompt.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}