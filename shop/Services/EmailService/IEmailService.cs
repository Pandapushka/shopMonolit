using shop.Model.Email;

namespace shop.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
        Task SendWelcomeEmailAsync(string userEmail, string userName);
        Task SendPasswordResetEmailAsync(string userEmail, string resetLink);
        Task SendOrderConfirmationEmailAsync(string userEmail, string userName, int orderId, double totalAmount, DateTime orderDate);
    }
}
