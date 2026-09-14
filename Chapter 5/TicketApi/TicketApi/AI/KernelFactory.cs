using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

public static class KernelFactory
{
    public static Kernel Create(IConfiguration config)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddAzureOpenAIChatCompletion(
            deploymentName: config["AzureOpenAI:Deployment"],
            endpoint: config["AzureOpenAI:Endpoint"],
            apiKey: config["AzureOpenAI:ApiKey"]
        );

        var kernel = builder.Build();

        kernel.Plugins.AddFromType<TicketPlugin>("Ticket");

        return kernel;
    }
}