namespace TextSummarizerApi.Services
{
    public interface ITextSummarizer
    {
        Task<string> SummarizeAsync(string text, CancellationToken ct = default);
    }

}
