using Microsoft.AspNetCore.Mvc;
using Ecomerce.API.Models;
using Ecomerce.API.Services;

namespace Ecomerce.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        private readonly INotificationService _notificationService;

        public OrdersController(IOpenAIService openAIService, INotificationService notificationService)
        {
            _openAIService = openAIService;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest request)
        {
            if (request.Items is null || request.Items.Count == 0)
            {
                return BadRequest("The order must contain at least one item.");
            }

            var orderId = Guid.NewGuid();
            var analysis = await _openAIService.AnalyzeOrderAsync(request, orderId);

            var department = analysis.Department;
            var totalAmount = request.Items.Sum(item => item.UnitPrice * item.Quantity);

            var emailTo = await _notificationService.NotifyDepartmentAsync(department, orderId.ToString(), totalAmount);

            var response = new OrderResponse
            {
                OrderId = orderId,
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Department = department,
                Justification = analysis.Justification,
                Priority = analysis.Priority,
                BusinessImpact = analysis.BusinessImpact,
                EmailSentTo = emailTo
            };

            return Ok(response);
        }
    }
}
