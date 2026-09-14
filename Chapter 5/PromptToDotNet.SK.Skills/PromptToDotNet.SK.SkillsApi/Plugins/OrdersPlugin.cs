using Microsoft.SemanticKernel;
using PromptToDotNet.SK.SkillsApi.Services;
using System.ComponentModel;

namespace PromptToDotNet.SK.SkillsApi.Plugins
{
    public sealed class OrdersPlugin
    {
        private readonly IOrdersService _orders;

        public OrdersPlugin(IOrdersService orders) => _orders = orders;

        [KernelFunction]
        [Description("Gets the current status of an order by its order number.")]
        public async Task<string> GetOrderStatusAsync(
            [Description("The order number, for example: SO-10293.")] string orderNumber,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return "Order number is required.";
            }

            if (orderNumber.Length > 32)
            {
                return "Order number is too long.";
            }

            return await _orders.GetStatusAsync(orderNumber, ct);
        }
    }
}
