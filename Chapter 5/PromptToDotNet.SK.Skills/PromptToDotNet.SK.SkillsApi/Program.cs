using System.Text.Json;
using Microsoft.SemanticKernel;
using PromptToDotNet.SK.SkillsApi.Models;
using PromptToDotNet.SK.SkillsApi.Plugins;
using PromptToDotNet.SK.SkillsApi.Services;

#pragma warning disable SKEXP0010

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Backend services
builder.Services.AddSingleton<IOrdersService, OrdersService>();

// Plugins / Skills (C#)
builder.Services.AddSingleton<OrdersPlugin>();
builder.Services.AddSingleton<SupportAssistantPrompts>();

// Kernel (singleton)
builder.Services.AddSingleton(sp =>
{
    var modelId = builder.Configuration["AI:ModelId"] ?? "gpt-4";
    var apiKey = builder.Configuration["AI:ApiKey"];

    if (string.IsNullOrWhiteSpace(apiKey))
        throw new InvalidOperationException("Missing AI:ApiKey (use User Secrets or environment variables).");

    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddOpenAIChatCompletion(
        modelId: modelId,
        apiKey: apiKey
    );

    var kernel = kernelBuilder.Build();

    // Only add real tools (native functions) as plugins
    kernel.Plugins.AddFromObject(sp.GetRequiredService<OrdersPlugin>(), "Orders");

    return kernel;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Keep redirect to Swagger, but hide it from the Swagger document
app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.MapPost("/support/summarize",
    async (Kernel kernel, SupportAssistantPrompts prompts, TicketRequest request, CancellationToken ct) =>
    {
        var args = new KernelArguments
        {
            ["ticketText"] = request.TicketText
        };

        var result = await kernel.InvokePromptAsync(
            prompts.SummarizeTicketPrompt(),
            args,
            cancellationToken: ct
        );

        var raw = result.ToString();

        // Expected JSON: { "summary": ["...", "...", "...", "..."] }
        try
        {
            using var doc = JsonDocument.Parse(raw);

            var summaryArray = doc.RootElement.GetProperty("summary")
                .EnumerateArray()
                .Select(x => x.GetString() ?? string.Empty)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            return Results.Ok(new { Summary = summaryArray });
        }
        catch
        {
            // Fallback if the model returns plain text
            var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return Results.Ok(new { Summary = lines });
        }
    });

app.MapPost("/support/draft",
    async (Kernel kernel, SupportAssistantPrompts prompts, TicketRequest request, CancellationToken ct) =>
    {
        var args = new KernelArguments
        {
            ["ticketText"] = request.TicketText,
            ["context"] = request.Context ?? string.Empty
        };

        var result = await kernel.InvokePromptAsync(
            prompts.DraftReplyPrompt(),
            args,
            cancellationToken: ct
        );

        var raw = result.ToString();

        // Expected JSON: { "replyLines": ["...", "..."] }
        try
        {
            using var doc = JsonDocument.Parse(raw);

            var replyLines = doc.RootElement.GetProperty("replyLines")
                .EnumerateArray()
                .Select(x => x.GetString() ?? string.Empty)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            return Results.Ok(new { ReplyLines = replyLines });
        }
        catch
        {
            // Fallback if the model returns plain text
            var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return Results.Ok(new { ReplyLines = lines });
        }
    });

app.MapGet("/orders/{orderNumber}/status",
    async (Kernel kernel, string orderNumber, CancellationToken ct) =>
    {
        var args = new KernelArguments
        {
            ["orderNumber"] = orderNumber
        };

        var result = await kernel.InvokeAsync("Orders", "GetOrderStatusAsync", args, ct);
        return Results.Ok(new { OrderNumber = orderNumber, Status = result.ToString() });
    });

app.Run();
