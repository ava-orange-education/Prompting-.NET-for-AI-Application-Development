using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace PromptToDotNet.SK.SkillsApi.Plugins;

public sealed class SupportAssistantPrompts
{
    [KernelFunction]
    [Description("Summarizes a support ticket into exactly four bullet-style items as JSON.")]
    public string SummarizeTicketPrompt() => """
You are a support assistant for an enterprise SaaS product.

Task:
Summarize the ticket into exactly 4 short bullet-style items.

Rules:
- Be specific and factual.
- Do not invent details.
- If key info is missing, include one item stating what is missing.
- Each item must start with an uppercase letter.
- Keep each item under 18 words.

Output:
Return valid JSON only using this exact shape:
{ "summary": ["...", "...", "...", "..."] }

Ticket:
{{$ticketText}}
""";

    [KernelFunction]
    [Description("Drafts a professional support reply as JSON lines that are easy to render.")]
    public string DraftReplyPrompt() => """
You are a senior support agent.

Task:
Write a professional reply that is polite, direct, and actionable.

Constraints:
- Do not mention internal systems.
- If you reference an order, ask for the order number if missing.
- Keep the total reply under 150 words.
- Each line must start with an uppercase letter.

Output:
Return valid JSON only using this exact shape:
{ "replyLines": ["...", "..."] }

Ticket:
{{$ticketText}}

Optional Context:
{{$context}}
""";
}
