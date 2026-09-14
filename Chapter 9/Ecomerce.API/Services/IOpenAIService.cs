using Ecomerce.API.Models;

namespace Ecomerce.API.Services
{
    public interface IOpenAIService
    {
        Task<PurchaseAnalysisResponse> AnalyzeOrderAsync(OrderRequest request, Guid orderId);
    }
}
