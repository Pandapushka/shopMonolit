using Microsoft.Extensions.Options;
using shop.Model.Email;

namespace shop.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendOrderConfirmationEmailAsync(string userEmail, string userName, int orderId, double totalAmount, DateTime orderDate)
        {
            var body = $@"
                <h2>Здравствуйте, {userName}!</h2>
                <p>Ваш заказ успешно оформлен. Спасибо за покупку!</p>
    
                <ul>
                    <li><strong>Номер заказа:</strong> #{orderId}</li>
                    <li><strong>Дата:</strong> {orderDate:dd.MM.yyyy HH:mm}</li>
                    <li><strong>Сумма заказа:</strong> {totalAmount:F2} ₽</li>
                </ul>

                <p>Мы уже начали обработку вашего заказа. Скоро с вами свяжутся для уточнения деталей доставки.</p>
    
                <hr>
                <small>Это письмо отправлено автоматически. Не отвечайте на него.</small>";

            var message = new EmailMessage
            {
                To = userEmail,
                Subject = $"Ваш заказ №{orderId} оформлен!",
                Body = body
            };

            await SendEmailAsync(message);
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var mailMessage = new MimeKit.MimeMessage();
            mailMessage.From.Add(new MimeKit.MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
            mailMessage.To.Add(MimeKit.MailboxAddress.Parse(message.To));
            mailMessage.Subject = message.Subject;
            mailMessage.Body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(mailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SendWelcomeEmailAsync(string userEmail, string userName)
        {
            var body = $@"
            <h2>Добро пожаловать, {userName}!</h2>
            <p>Спасибо за регистрацию в нашем магазине.</p>
            <p>Ваш email: {userEmail}</p>
            <hr>
            <small>Это письмо отправлено автоматически. Не отвечайте на него.</small>";

            var message = new EmailMessage
            {
                To = userEmail,
                Subject = "Добро пожаловать в наш магазин!",
                Body = body
            };

            await SendEmailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string userEmail, string resetLink)
        {
            var body = $@"
            <h2>Сброс пароля</h2>
            <p>Вы запросили сброс пароля. Нажмите на ссылку ниже:</p>
            <a href='{resetLink}' target='_blank'>Сбросить пароль</a>
            <p>Ссылка действительна 1 час.</p>
            <hr>
            <small>Если вы не запрашивали сброс — проигнорируйте это письмо.</small>";

            var message = new EmailMessage
            {
                To = userEmail,
                Subject = "Сброс пароля",
                Body = body
            };

            await SendEmailAsync(message);
        }
    }
}

