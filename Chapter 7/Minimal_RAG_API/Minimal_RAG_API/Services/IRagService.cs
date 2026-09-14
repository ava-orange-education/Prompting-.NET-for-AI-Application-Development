using Minimal_RAG_API.Models;

namespace Minimal_RAG_API.Services
{
    public interface IRagService
    {
        Task<RagQueryResponse> AnswerAsync(RagQueryRequest request);
    }
}