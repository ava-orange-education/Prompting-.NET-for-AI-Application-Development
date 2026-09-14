namespace PromptToDotNet.SK.SkillsApi.Services
{
    public sealed class OrdersService : IOrdersService
    {
        private static readonly Dictionary<string, string> _statuses = new(StringComparer.OrdinalIgnoreCase)
        {
            ["SO-10293"] = "Shipped",
            ["SO-10001"] = "Processing",
            ["SO-20007"] = "Delayed"
        };

        public Task<string> GetStatusAsync(string orderNumber, CancellationToken ct)
        {
            _statuses.TryGetValue(orderNumber.Trim(), out var status);
            return Task.FromResult(status ?? "Unknown");
        }
    }
}
