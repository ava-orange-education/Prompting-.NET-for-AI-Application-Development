namespace Minimal_RAG_API.Services
{
    public class VectorStore : IVectorStore
    {
        private readonly List<(string chunk, float[] embedding)> _store = new();
        private readonly IEmbeddingService _embedder;

        public VectorStore(IEmbeddingService embedder)
        {
            _embedder = embedder;

            // Load some mock documents at construction time for demo purposes
            LoadDocuments(new[]
            {
                "Dotnet 10 introduces many performance improvements and small API updates.",
                "Minimal APIs are a lightweight way to build HTTP APIs in ASP.NET Core.",
                "Retrieval-Augmented Generation combines retrieval of context with a language model.",
                "Vector stores hold embeddings and support similarity search via cosine similarity.",
                "Chunking documents helps to return focused context for a given query."
            });
        }

        public void LoadDocuments(IEnumerable<string> chunks)
        {
            foreach (var c in chunks)
            {
                var emb = _embedder.Embed(c);
                _store.Add((c, emb));
            }
        }

        public (string chunk, double score)[] SimilaritySearch(float[] queryEmbedding, int topK = 3)
        {
            var results = new List<(string, double)>();
            foreach (var (chunk, emb) in _store)
            {
                var score = CosineSimilarity(queryEmbedding, emb);
                results.Add((chunk, score));
            }

            return results.OrderByDescending(r => r.Item2).Take(topK).ToArray();
        }

        private static double CosineSimilarity(float[] a, float[] b)
        {
            if (a.Length != b.Length) return 0;
            double dot = 0, na = 0, nb = 0;
            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                na += a[i] * a[i];
                nb += b[i] * b[i];
            }
            if (na == 0 || nb == 0) return 0;
            return dot / (Math.Sqrt(na) * Math.Sqrt(nb));
        }
    }
}