namespace Minimal_RAG_API.Services
{
    public class LlmService : ILlmService
    {
        // Simple deterministic "generation" that we can explain in a book chapter.
        public string Generate(string question, string[] contextChunks)
        {
            // Build a pseudo-answer by echoing context and giving a short summary
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Synthesized Answer:");
            sb.AppendLine();

            if (contextChunks != null && contextChunks.Length > 0)
            {
                sb.AppendLine("Context used:");
                foreach (var c in contextChunks)
                {
                    sb.AppendLine("- " + c);
                }
                sb.AppendLine();
            }

            sb.AppendLine($"Question: {question}");
            sb.AppendLine();
            sb.AppendLine("Answer: ");
            sb.AppendLine(Summarize(question, contextChunks));

            return sb.ToString();
        }

        private string Summarize(string question, string[] context)
        {
            // Naive heuristic: mention keyword matches or give a generic fallback.
            if (context != null && context.Length > 0)
            {
                var keywords = context.SelectMany(c => c.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    .GroupBy(w => w.ToLowerInvariant())
                    .OrderByDescending(g => g.Count())
                    .Take(3)
                    .Select(g => g.Key)
                    .ToArray();

                return $"Based on the retrieved information (top keywords: {string.Join(", ", keywords)}), here's a concise response addressing your question.";
            }

            return "I don't have enough information to answer precisely. Try providing more context.";
        }
    }
}