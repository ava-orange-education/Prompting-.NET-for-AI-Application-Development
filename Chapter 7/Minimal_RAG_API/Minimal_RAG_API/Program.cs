using Minimal_RAG_API.Models;
using Minimal_RAG_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Register RAG services
builder.Services.AddScoped<IEmbeddingService, EmbeddingService>();
builder.Services.AddScoped<IVectorStore, VectorStore>();
builder.Services.AddScoped<ILlmService, LlmService>();
builder.Services.AddScoped<IRagService, RagService>();

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OpenAPI for exploration (kept simple)
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

// RAG endpoint
app.MapPost("/api/rag/query", async (IRagService ragService, RagQueryRequest request) =>
{
    if (request is null || string.IsNullOrWhiteSpace(request.Question))
    {
        return Results.BadRequest(new { error = "Question is required." });
    }

    var response = await ragService.AnswerAsync(request);
    return Results.Ok(response);
}).WithName("RagQuery").WithOpenApi();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
