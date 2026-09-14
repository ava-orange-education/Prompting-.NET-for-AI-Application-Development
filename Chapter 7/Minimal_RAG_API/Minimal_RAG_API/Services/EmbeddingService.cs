using System.Text;

namespace Minimal_RAG_API.Services
{
    public class EmbeddingService : IEmbeddingService
    {
        // Simple, deterministic mock embedding: map chars to normalized floats
        public float[] Embed(string text)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<float>();

            // Use a fixed-length embedding for simplicity
            const int dim = 32;
            var vec = new float[dim];

            // Fill vector by iterating chars and adding normalized values
            var bytes = Encoding.UTF8.GetBytes(text);
            for (int i = 0; i < bytes.Length; i++)
            {
                vec[i % dim] += (bytes[i] % 128) / 128f;
            }

            // Normalize
            var norm = Math.Sqrt(vec.Select(v => v * v).Sum());
            if (norm > 0)
            {
                for (int i = 0; i < vec.Length; i++) vec[i] = (float)(vec[i] / norm);
            }

            return vec;
        }
    }
}   