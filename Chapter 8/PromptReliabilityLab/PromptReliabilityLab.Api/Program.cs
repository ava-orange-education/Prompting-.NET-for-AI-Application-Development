using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.SemanticKernel;
using PromptReliabilityLab.AI.Configuration;
using PromptReliabilityLab.AI.Services;
using PromptReliabilityLab.Application.Contracts;
using PromptReliabilityLab.Application.Models;
using PromptReliabilityLab.Application.Workflows;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureOpenAIOptions>(
    builder.Configuration.GetSection("AzureOpenAI"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Prompt Reliability Lab API",
        Version = "v1"
    });
});

builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;

    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: options.DeploymentName,
        endpoint: options.Endpoint,
        apiKey: options.ApiKey);

    return kernelBuilder.Build();
});

builder.Services.AddScoped<IReviewContextProvider, StaticReviewContextProvider>();
builder.Services.AddScoped<IReviewClassifier, SimpleReviewClassifier>();
builder.Services.AddScoped<IReviewGenerator, SemanticKernelReviewGenerator>();
builder.Services.AddScoped<ISupportReviewWorkflow, SupportReviewWorkflow>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/support-review/run",
    async (
        ReviewWorkflowRequest request,
        ISupportReviewWorkflow workflow,
        CancellationToken cancellationToken) =>
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return Results.BadRequest(new
            {
                error = "Content is required."
            });
        }

        var result = await workflow.RunAsync(
            request,
            cancellationToken);

        return Results.Ok(result);
    })
    .WithName("RunSupportReview")
    .WithTags("Support Review")
    .Accepts<ReviewWorkflowRequest>("application/json")
    .Produces<SupportReviewResult>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

app.Run();