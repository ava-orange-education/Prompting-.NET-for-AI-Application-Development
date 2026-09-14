using ChatbotConsole.Configuration;
using ChatbotConsole.Models;
using ChatbotConsole.Services;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

var options = config
    .GetSection("AzureOpenAI")
    .Get<AzureOpenAIOptions>()!;

IChatService chatService = new AzureOpenAIChatService(options);

Console.WriteLine("Azure OpenAI Chat (type 'exit' to exit)");
Console.WriteLine("---------------------------------------");

var messages = new List<ChatMessage>
{
    new() { Role = "system", Content = "You are a helpful and straightforward assistant." }
};

while (true)
{
    Console.Write("\nYou: ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    messages.Add(new ChatMessage { Role = "user", Content = input });

    var response = await chatService.SendAsync(messages);

    Console.WriteLine($"\nAssistant: {response}");

    messages.Add(new ChatMessage { Role = "assistant", Content = response });
}
