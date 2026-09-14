namespace Minimal_RAG_API.Services
{
    public interface ILlmService
    {
        // Simulate an LLM generation given a prompt and context
        string Generate(string question, string[] contextChunks);
    }
}