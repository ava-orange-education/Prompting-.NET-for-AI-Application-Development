namespace TicketApi.AI.Prompts
{
    public static class TicketPrompt
    {
        public const string Analyze = """
        Return ONLY JSON format:

        {
          "category": "",
          "priority": "",
          "summary": ""
        }

        Ticket:
        {{$input}}
        """;
    }
}
