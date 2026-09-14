using Microsoft.AspNetCore.Mvc;
using Ecomerce.API.Models;
using Ecomerce.API.Services;

namespace Ecomerce.API.Controllers
{
    [ApiController]
    [Route("api/purchase-analysis")]
    public class PurchaseAnalysisController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        private readonly INotificationService _notificationService;

        public PurchaseAnalysisController(IOpenAIService openAIService, INotificationService notificationService)
        {
            _openAIService = openAIService;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Analyze([FromBody] OrderRequest request)
        {
            if (request.Items is null || request.Items.Count == 0)
            {
                return BadRequest("The order must contain at least one item.");
            }

            var orderId = Guid.NewGuid();
            var result = await _openAIService.AnalyzeOrderAsync(request, orderId);

            await _notificationService.NotifyDepartmentAsync(
                result.Department, orderId.ToString(),
                request.Items.Sum(item => item.UnitPrice * item.Quantity));

            return Ok(result);
        }
    }
}
