namespace Ecomerce.API.Models
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Justification { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string BusinessImpact { get; set; } = string.Empty;
        public string EmailSentTo { get; set; } = string.Empty;
    }
}
