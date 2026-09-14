using Microsoft.SemanticKernel;

public class TicketPlugin
{
    [KernelFunction]
    public string GenerateTicketId()
    {
        return Guid.NewGuid().ToString();
    }
}