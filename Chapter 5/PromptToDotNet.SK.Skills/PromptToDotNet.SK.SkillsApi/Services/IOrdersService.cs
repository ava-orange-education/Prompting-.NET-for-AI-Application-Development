namespace PromptToDotNet.SK.SkillsApi.Services
{
    public interface IOrdersService
    {
        Task<string> GetStatusAsync(string orderNumber, CancellationToken ct);
    }
}
