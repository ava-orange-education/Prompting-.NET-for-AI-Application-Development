namespace Minimal_RAG_API.Models
{
    public record RagQueryResponse(string Answer, string[] RetrievedChunks, double[] Scores);
}