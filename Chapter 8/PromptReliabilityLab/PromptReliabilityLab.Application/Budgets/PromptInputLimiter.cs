namespace PromptReliabilityLab.Application.Budgets;

public static class PromptInputLimiter
{
    public static string Cap(string value, int maxCharacters)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var trimmed = value.Trim();

        return trimmed.Length <= maxCharacters
            ? trimmed
            : trimmed[..maxCharacters];
    }

    public static bool WasTrimmed(string originalValue, string cappedValue)
    {
        if (string.IsNullOrWhiteSpace(originalValue))
            return false;

        return originalValue.Trim().Length > cappedValue.Length;
    }
}