namespace Ecomerce.API.Models
{
    public class PurchaseAnalysisResponse
    {
        public Guid OrderId { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Justification { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string BusinessImpact { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
