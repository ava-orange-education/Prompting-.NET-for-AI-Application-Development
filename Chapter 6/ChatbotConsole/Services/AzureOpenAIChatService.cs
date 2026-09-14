using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ChatbotConsole.Configuration;
using ChatbotConsole.Models;

namespace ChatbotConsole.Services;

/// <summary>
/// Azure OpenAI implementation of <see cref="IChatService"/>.
/// It is responsible for calling the Azure OpenAI REST API and
/// returning the assistant's message content.
/// </summary>
public class AzureOpenAIChatService : IChatService
{
    private readonly HttpClient _httpClient;
    private readonly AzureOpenAIOptions _options;

    /// <summary>
    /// Create a new instance of the chat service using the provided options.
    /// The service configures an internal HttpClient with the required
    /// base address and API key header.
    /// </summary>
    /// <param name="options">Configuration options for Azure OpenAI.</param>
    public AzureOpenAIChatService(AzureOpenAIOptions options)
    {
        _options = options;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_options.Endpoint)
        };

        // Add the API key header required by Azure OpenAI
        _httpClient.DefaultRequestHeaders.Add("api-key", _options.ApiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Send conversation messages to the configured deployment and return
    /// the assistant's reply text.
    /// </summary>
    /// <param name="messages">Conversation history to send to the model.</param>
    public async Task<string> SendAsync(List<ChatMessage> messages)
    {
        var requestBody = new
        {
            messages = messages.Select(m => new
            {
                role = m.Role,
                content = m.Content
            }),
            temperature = 0.7,
            max_tokens = 500
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // POST to the chat completions endpoint for the specified deployment
        var response = await _httpClient.PostAsync(
            $"openai/deployments/{_options.DeploymentName}/chat/completions?api-version={_options.ApiVersion}",
            content);

        // Throw a descriptive exception when the request fails
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);

        // Extract and return the assistant's message content
        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}
