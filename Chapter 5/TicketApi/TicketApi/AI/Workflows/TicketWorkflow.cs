using Microsoft.SemanticKernel;
using System.Text.Json;
using TicketApi.AI.Prompts;
using TicketApi.Model;

public class TicketWorkflow
{
    private readonly Kernel _kernel;

    public TicketWorkflow(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<TicketResponse> Process(string message)
    {

        var function = _kernel.CreateFunctionFromPrompt(TicketPrompt.Analyze);

        var result = await _kernel.InvokeAsync(function, new()
        {
            ["input"] = message
        });

        var ticket = JsonSerializer.Deserialize<TicketResponse>(
            result.ToString()
        );

        return ticket;
    }
}