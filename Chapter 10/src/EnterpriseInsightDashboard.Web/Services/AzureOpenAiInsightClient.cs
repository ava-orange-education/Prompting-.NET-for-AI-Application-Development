using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EnterpriseInsightDashboard.Web.Models;
using Microsoft.Extensions.Options;

namespace EnterpriseInsightDashboard.Web.Services;

public sealed class AzureOpenAiInsightClient(
    HttpClient httpClient,
    IOptions<AzureOpenAiOptions> options,
    ILogger<AzureOpenAiInsightClient> logger) : IAiInsightClient
{
    private readonly AzureOpenAiOptions _options = options.Value;

    public async Task<InsightGenerationResponse> GenerateAsync(
        InsightGenerationRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.Endpoint) ||
            string.IsNullOrWhiteSpace(_options.DeploymentName) ||
            string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            logger.LogWarning("Azure OpenAI is configured as provider, but one or more settings are missing.");
            return new InsightGenerationResponse(
                RawContent: "{}",
                Provider: "AzureOpenAI-NotConfigured",
                DurationMs: 0);
        }

        var stopwatch = Stopwatch.StartNew();
        var endpoint = _options.Endpoint.TrimEnd('/');
        var url = $"{endpoint}/openai/deployments/{_options.DeploymentName}/chat/completions?api-version={_options.ApiVersion}";

        var payload = new
        {
            messages = new[]
            {
                new { role = "system", content = "Return only valid JSON for the enterprise dashboard insight." },
                new { role = "user", content = request.Prompt }
            },
            max_tokens = request.MaxOutputTokens,
            temperature = 0.2
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequest.Headers.Add("api-key", _options.ApiKey);
        httpRequest.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");
        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        var raw = await response.Content.ReadAsStringAsync(cancellationToken);

        stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning(
                "Azure OpenAI returned status code {StatusCode}.",
                response.StatusCode);

            return new InsightGenerationResponse(
                RawContent: "{}",
                Provider: "AzureOpenAI-Error",
                DurationMs: stopwatch.ElapsedMilliseconds);
        }

        var content = ExtractAssistantContent(raw);

        return new InsightGenerationResponse(
            RawContent: content,
            Provider: "AzureOpenAI",
            DurationMs: stopwatch.ElapsedMilliseconds);
    }

    private static string ExtractAssistantContent(string raw)
    {
        using var document = JsonDocument.Parse(raw);
        var root = document.RootElement;

        var choices = root.GetProperty("choices");
        if (choices.GetArrayLength() == 0)
        {
            return "{}";
        }

        return choices[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "{}";
    }
}
