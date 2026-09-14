using PromptReview.Application.Abstractions;
using PromptReview.Application.Services;
using PromptReview.Contracts.Models;
using PromptReview.Infrastructure.Ai;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<PromptReviewService>();
builder.Services.AddScoped<IAiModelClient, FakeAiModelClient>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/api/prompt-review", async (
    PromptReviewRequest request,
    PromptReviewService promptReviewService,
    CancellationToken cancellationToken) =>
{
    var response = await promptReviewService.ReviewAsync(request, cancellationToken);

    return Results.Ok(response);
});

app.Run();