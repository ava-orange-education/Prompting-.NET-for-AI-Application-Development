namespace Ecomerce.API.Services
{
    public interface INotificationService
    {
        Task<string> NotifyDepartmentAsync(string department, string orderId, decimal totalAmount);
    }
}
