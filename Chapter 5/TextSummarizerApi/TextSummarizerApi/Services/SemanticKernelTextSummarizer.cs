using Microsoft.SemanticKernel;

namespace TextSummarizerApi.Services
{

    public sealed class SemanticKernelTextSummarizer : ITextSummarizer
    {
        private readonly Kernel _kernel;
        private readonly KernelFunction _summarizeFn;

        private const string SummarizePrompt = """
    You are an assistant that summarizes business text for software teams.

    Output rules:
    - Use English.
    - Keep it under 120 words.
    - Return exactly these sections in this order:

    Summary:
    Key points:
    - (bullets)

    Action items:
    - (bullets)

    Text:
    {{$input}}
    """;

        public SemanticKernelTextSummarizer(Kernel kernel)
        {
            _kernel = kernel;

            _summarizeFn = _kernel.CreateFunctionFromPrompt(
                promptTemplate: SummarizePrompt,
                functionName: "SummarizeText"
            );
        }

        public async Task<string> SummarizeAsync(string text, CancellationToken ct = default)
        {
            var result = await _kernel.InvokeAsync(
                _summarizeFn,
                new KernelArguments { ["input"] = text },
                cancellationToken: ct
            );

            return result.GetValue<string>() ?? result.ToString();
        }
    }

}
