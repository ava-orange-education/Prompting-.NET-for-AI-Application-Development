using Minimal_RAG_API.Models;

namespace Minimal_RAG_API.Services
{
    public class RagService : IRagService
    {
        private readonly IEmbeddingService _emb;
        private readonly IVectorStore _store;
        private readonly ILlmService _llm;

        public RagService(IEmbeddingService emb, IVectorStore store, ILlmService llm)
        {
            _emb = emb;
            _store = store;
            _llm = llm;
        }

        public Task<RagQueryResponse> AnswerAsync(RagQueryRequest request)
        {
            // 1. Create embedding for the query
            var qEmb = _emb.Embed(request.Question);

            // 2. Retrieve relevant chunks from the vector store
            var results = _store.SimilaritySearch(qEmb, topK: 3);
            var chunks = results.Select(r => r.chunk).ToArray();
            var scores = results.Select(r => r.score).ToArray();

            // 3. Build context and call (simulated) LLM
            var answer = _llm.Generate(request.Question, chunks);

            // 4. Return structured response
            var response = new RagQueryResponse(answer, chunks, scores);
            return Task.FromResult(response);
        }
    }
}