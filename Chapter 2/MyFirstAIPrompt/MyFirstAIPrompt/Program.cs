using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

// Azure OpenAI configuration
string endpoint = "https://your-resource.openai.azure.com/openai/deployments/your-model/chat/completions?api-version=2024-02-01";
string apiKey = "YOUR_API_KEY";

// Create HttpClient instance
using var client = new HttpClient();
client.DefaultRequestHeaders.Add("api-key", apiKey);

// Create the request body
var requestBody = new
{
    model = "gpt-4",
    messages = new[]
    {
        new { role = "system", content = "You are a helpful assistant specialized in .NET 10 and AI." },
        new { role = "user", content = "Write a short example of an async method in C#." }
    },
    temperature = 0.7,
    max_tokens = 200
};

try
{
    var response = await client.PostAsJsonAsync(endpoint, requestBody);

    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Error: {response.StatusCode}");
        var error = await response.Content.ReadAsStringAsync();
        Console.WriteLine(error);
        return;
    }

    var json = await response.Content.ReadAsStringAsync();
    using var document = JsonDocument.Parse(json);
    var content = document
        .RootElement
        .GetProperty("choices")[0]
        .GetProperty("message")
        .GetProperty("content")
        .GetString();

    Console.WriteLine("Model response:");
    Console.WriteLine(content);
}
catch (Exception ex)
{
    Console.WriteLine("An unexpected error occurred:");
    Console.WriteLine(ex.Message);
}

