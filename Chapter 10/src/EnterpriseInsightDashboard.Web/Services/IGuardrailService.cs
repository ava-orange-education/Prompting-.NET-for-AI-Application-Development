using EnterpriseInsightDashboard.Web.Models;

namespace EnterpriseInsightDashboard.Web.Services;

public interface IGuardrailService
{
    GuardrailDecision ValidatePrompt(string prompt, PromptBudgetOptions budget);
}
