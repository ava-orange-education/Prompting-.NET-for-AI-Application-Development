using Microsoft.SemanticKernel;
using TextSummarizerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Kernel registration
builder.Services.AddSingleton(sp =>
{
    var kb = Kernel.CreateBuilder();

    // If you installed the OpenAI connector package, this extension method should resolve.
#pragma warning disable SKEXP0070
    kb.AddOpenAIChatCompletion(
        modelId: "gpt-4o-mini",
        apiKey: builder.Configuration["OpenAI:ApiKey"]!
    );

    return kb.Build();
});

// Summarizer service
builder.Services.AddSingleton<ITextSummarizer, SemanticKernelTextSummarizer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/summaries", async (SummarizeRequest req, ITextSummarizer summarizer, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(req.Text))
        return Results.BadRequest("Text is required.");

    if (req.Text.Length > 30_000)
        return Results.BadRequest("Text is too long.");

    var summary = await summarizer.SummarizeAsync(req.Text, ct);
    return Results.Ok(new SummarizeResponse(summary));
});

app.Run();

public sealed record SummarizeRequest(string Text);
public sealed record SummarizeResponse(string Summary);
