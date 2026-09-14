var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register application services
builder.Services.AddSingleton<ExpenseAPI.Services.ITransactionService, ExpenseAPI.Services.TransactionService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Make the app listen on all network interfaces on port 5081 so it is reachable from other devices on the LAN.
var app = builder.Build();
// Configure the HTTP request pipeline.
// Expose OpenAPI/Swagger UI unconditionally so it can be used for testing.
app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
