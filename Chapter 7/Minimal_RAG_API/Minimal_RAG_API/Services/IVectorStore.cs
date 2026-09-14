namespace Minimal_RAG_API.Services
{
    public interface IVectorStore
    {
        // Add documents (chunked) to the store
        void LoadDocuments(IEnumerable<string> chunks);

        // Retrieve top-k relevant chunks given a query embedding
        (string chunk, double score)[] SimilaritySearch(float[] queryEmbedding, int topK = 3);
    }
}