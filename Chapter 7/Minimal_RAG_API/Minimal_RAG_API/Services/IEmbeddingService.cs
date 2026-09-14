namespace Minimal_RAG_API.Services
{
    public interface IEmbeddingService
    {
        // Convert text to a mock embedding (float array)
        float[] Embed(string text);
    }
}