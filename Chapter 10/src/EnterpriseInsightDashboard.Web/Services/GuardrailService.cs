using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public sealed class GuardrailService : IGuardrailService
{
    public GuardrailDecision ValidatePrompt(string prompt, PromptBudgetOptions budget)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return new GuardrailDecision(false, "The prompt was empty.");
        }

        if (prompt.Length > budget.MaxInputCharacters)
        {
            return new GuardrailDecision(
                false,
                $"The prompt exceeded the configured limit of {budget.MaxInputCharacters} characters.");
        }

        return new GuardrailDecision(true, null);
    }
}
