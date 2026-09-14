using System.Text.Json;
using PromptReliabilityLab.Application.Models;
using PromptReliabilityLab.Application.Validation;

namespace PromptReliabilityLab.AI.Parsing;

public static class ReviewResultParser
{
    public static bool TryParse(
        string json,
        string promptVersion,
        out SupportReviewResult result,
        out string reason)
    {
        result = default!;
        reason = string.Empty;

        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var parsed = new SupportReviewResult(
                Title: Read(root, "title"),
                Category: Read(root, "category"),
                Priority: Read(root, "priority"),
                Summary: Read(root, "summary"),
                RecommendedAction: Read(root, "recommendedAction"),
                UsedFallback: false,
                PromptVersion: promptVersion,
                ValidationWarnings: []);

            if (ReviewResultValidator.IsValid(parsed, out var warnings))
            {
                result = parsed;
                return true;
            }

            reason = string.Join(" ", warnings);
            return false;
        }
        catch (Exception ex)
        {
            reason = ex.Message;
            return false;
        }
    }

    private static string Read(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var value))
            throw new InvalidOperationException($"Missing field: {name}");

        return value.GetString()?.Trim() ?? string.Empty;
    }
}