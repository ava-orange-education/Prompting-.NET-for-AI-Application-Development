var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return KernelFactory.Create(config);
});
// Add services to the container.

builder.Services.AddControllers();
// Register workflow for dependency injection
builder.Services.AddTransient<TicketWorkflow>();
// Add OpenAPI support (built-in for .NET 10)
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure OpenAPI endpoints (always available)
app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
