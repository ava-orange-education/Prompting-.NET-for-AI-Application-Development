namespace Ecomerce.API.Services
{
    public class SimulatedEmailService : INotificationService
    {
        private readonly ILogger<SimulatedEmailService> _logger;

        private static readonly Dictionary<string, string> DepartmentEmails = new()
        {
            { "IT", "it@company.com" },
            { "Human Resources", "hr@company.com" },
            { "Finance", "finance@company.com" },
            { "Operations", "operations@company.com" },
            { "Marketing", "marketing@company.com" }
        };

        public SimulatedEmailService(ILogger<SimulatedEmailService> logger)
        {
            _logger = logger;
        }

        public Task<string> NotifyDepartmentAsync(string department, string orderId, decimal totalAmount)
        {
            var email = DepartmentEmails.GetValueOrDefault(department, "sin-asignar@empresa.com");

             var message = $"""
                To: {email}
                Subject: New Purchase Order Pending Approval - {orderId}

                Dear Approver,

                A new purchase order has been created and requires your review and approval.

                Purchase Order Details:
                - Order Number: {orderId}
                - Department: {department}
                - Total Amount: ${totalAmount:F2}
                - Status: Pending Approval

                Please review the purchase order and provide your approval at your earliest convenience so the process can continue.

                Best regards,
                Purchase Management System
                """;

            _logger.LogInformation("Correo enviado a {Email}:{NewLine}{Message}", email, Environment.NewLine, message);

            return Task.FromResult(email);
        }
    }
}
