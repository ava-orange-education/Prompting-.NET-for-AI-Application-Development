
using Azure;
using Azure.AI.OpenAI;
using ExpenseAPI.Models;
using OpenAI.Chat;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseAPI.Services;

public class TransactionService : ITransactionService
{
    private readonly List<Transaction> _transactions;
    private readonly AzureOpenAIClient _client;
    private readonly ChatClient _chatClient;
    private readonly string? _deploymentName;


    public TransactionService(IConfiguration config)
    {
        _transactions = new List<Transaction>
        {
            new Transaction { Id = 1, Description = "Office supplies", Category = "Supplies", Quantity = 3, UnitPrice = 12.50m, Total = 37.50m },
            new Transaction { Id = 2, Description = "Coffee", Category = "Food", Quantity = 10, UnitPrice = 2.30m, Total = 23.00m },
            new Transaction { Id = 3, Description = "Monthly subscription", Category = "Services", Quantity = 1, UnitPrice = 29.99m, Total = 29.99m },
            new Transaction { Id = 4, Description = "Laptop", Category = "Equipment", Quantity = 1, UnitPrice = 999.99m, Total = 999.99m },
            new Transaction { Id = 5, Description = "Phone", Category = "Equipment", Quantity = 1, UnitPrice = 799.99m, Total = 799.99m },
            new Transaction { Id = 6, Description = "MacBook", Category = "Equipment", Quantity = 1, UnitPrice = 1299.99m, Total = 1299.99m }
        };

        
        var endpoint = new Uri("url OpenIA");
        var deploymentName = "<your-deplayname>";
        var apiKey = "<your-api-key>";

        _client = new AzureOpenAIClient(
            endpoint,
            new AzureKeyCredential(apiKey)
        );

        _chatClient = _client.GetChatClient(deploymentName);
    }

    public async Task<List<TransactionLLM>> GetAll()
    {
        return await AskAsync(_transactions);
    }


    private async Task<List<TransactionLLM>> AskAsync(List<Transaction> list)
    {
        // 1. convert list to JSON
        var transactionsJson = JsonSerializer.Serialize(list);

        // 2. Well-defined prompt
        var prompt = $@"
            You are a financial assistant.

            Analyze the following transactions and calculate the percentage of total spending per category.

            Transactions:
            {transactionsJson}

            Return ONLY valid JSON in this format:
            [
              {{
                ""category"": ""string"",
                ""percentage"": number
              }}
            ]

            Rules:
            - No explanations
            - No markdown
            - Percentages must sum to 100
            ";

        var requestOptions = new ChatCompletionOptions()
        {
            MaxOutputTokenCount = 1000,
            Temperature = 0.2f // important: less creativity = more precision
        };

        var messages = new List<ChatMessage>()
        {
            new SystemChatMessage("You only return valid JSON."),
            new UserChatMessage(prompt),
        };

        // 3. Correct async call
        var response = await _chatClient.CompleteChatAsync(messages, requestOptions);

        var raw = response.Value.Content[0].Text;

        // 4. Clean up (in case the LLM inserts ```json fences)
        var cleanJson = raw
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        // 5. Basic validation
        if (!cleanJson.StartsWith("["))
            throw new Exception("The LLM did not return valid JSON");

        // 6. Safe deserialization
        var result = JsonSerializer.Deserialize<List<TransactionLLM>>(cleanJson,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result ?? new List<TransactionLLM>();
    }
}