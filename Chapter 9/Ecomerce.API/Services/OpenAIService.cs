using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ecomerce.API.Models;

namespace Ecomerce.API.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI:ApiKey is not configured.");
            _model = configuration["OpenAI:Model"] ?? "gpt-4o-mini";
        }

        public async Task<PurchaseAnalysisResponse> AnalyzeOrderAsync(OrderRequest request, Guid orderId)
        {
            var prompt = BuildPrompt(request);

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are a corporate purchasing assistant. Respond only in JSON format." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.3
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = JsonContent.Create(requestBody)
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {_apiKey}");

            var httpResponse = await _httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();

            var responseBody = await httpResponse.Content.ReadFromJsonAsync<OpenAiResponse>();

            var analysis = ParseResponse(responseBody?.Choices?[0]?.Message?.Content);

            return new PurchaseAnalysisResponse
            {
                OrderId = orderId,
                Department = analysis.Department,
                Justification = analysis.Justification,
                Priority = analysis.Priority,
                BusinessImpact = analysis.BusinessImpact,
                Status = "Analyzed"
            };
        }

        private static string BuildPrompt(OrderRequest request)
        {
            var items = string.Join("\n", request.Items.Select(i =>
                $"  - {i.ProductName} (x{i.Quantity}, ${i.UnitPrice} each)"));

            return $@"Classify the following purchase order and respond in JSON format with these exact keys: department, justification, priority, businessImpact.
                Rules:
                - Laptops, keyboards, monitors, headphones, and peripherals belong to TI.
                - Office equipment belongs to Operaciones.
                - Training and employee tools belong to Recursos Humanos.
                - Budget-related requests belong to Finanzas.
                - Advertising or campaign materials belong to Marketing.

                Priority levels: Baja, Media, Alta, Crítica.

                Purchase order items:
                {items}";
        }

        private static AnalysisResult ParseResponse(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return new AnalysisResult();

            try
            {
                var json = content.Contains("```")
                    ? content.Split("```")[1].Replace("json", "").Trim()
                    : content.Trim();

                return JsonSerializer.Deserialize<AnalysisResult>(json) ?? new AnalysisResult();
            }
            catch
            {
                return new AnalysisResult();
            }
        }

        private class AnalysisResult
        {
            public string Department { get; set; } = "Unknown";
            public string Justification { get; set; } = "Not available";
            public string Priority { get; set; } = "Media";
            public string BusinessImpact { get; set; } = "Not available";
        }

        private class OpenAiResponse
        {
            [JsonPropertyName("choices")]
            public List<Choice>? Choices { get; set; }
        }

        private class Choice
        {
            [JsonPropertyName("message")]
            public Message? Message { get; set; }
        }

        private class Message
        {
            [JsonPropertyName("content")]
            public string? Content { get; set; }
        }
    }
}
