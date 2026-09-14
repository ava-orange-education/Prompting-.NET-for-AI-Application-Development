using EnterpriseInsightDashboard.Web.Models;
using EnterpriseInsightDashboard.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.Configure<PromptBudgetOptions>(
    builder.Configuration.GetSection("PromptBudget"));
builder.Services.Configure<AzureOpenAiOptions>(
    builder.Configuration.GetSection("AzureOpenAI"));

builder.Services.AddHttpClient<AzureOpenAiInsightClient>();
builder.Services.AddSingleton<IEnterpriseDataService, FakeEnterpriseDataService>();
builder.Services.AddSingleton<IPromptBuilder, DashboardPromptBuilder>();
builder.Services.AddSingleton<IStructuredInsightValidator, StructuredInsightValidator>();
builder.Services.AddSingleton<IGuardrailService, GuardrailService>();
builder.Services.AddSingleton<IDashboardTelemetryWriter, DashboardTelemetryWriter>();

var provider = builder.Configuration["AiProvider"];

if (string.Equals(provider, "AzureOpenAI", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IAiInsightClient, AzureOpenAiInsightClient>();
}
else
{
    builder.Services.AddScoped<IAiInsightClient, SimulatedAiInsightClient>();
}

builder.Services.AddScoped<IDashboardInsightService, DashboardInsightService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.Run();
