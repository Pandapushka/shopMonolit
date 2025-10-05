using Microsoft.AspNetCore.Identity;
using shop.Data;
using shop.Model;
using shop.Services.EmailService;

namespace shop.Services.EmailConfirmationService
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly Random _random;

        public EmailConfirmationService(AppDbContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _random = new Random();
        }

        public async Task<string> GenerateConfirmationCodeAsync(string email)
        {
            var code = _random.Next(100000, 999999).ToString();

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.EmailConfirmationCode = code;
                user.EmailConfirmationCodeExpires = DateTime.UtcNow.AddMinutes(15);
                await _userManager.UpdateAsync(user);
            }

            return code;
        }

        public async Task<bool> VerifyConfirmationCodeAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            if (user.EmailConfirmationCode != code) return false;
            if (user.EmailConfirmationCodeExpires < DateTime.UtcNow) return false;

            user.EmailConfirmed = true;
            user.EmailConfirmationCode = null; 
            user.EmailConfirmationCodeExpires = null;

            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> SendConfirmationCodeAsync(string email, string code)
        {
            var emailMessage = new Model.Email.EmailMessage
            {
                To = email,
                Subject = "Код подтверждения email - Shop",
                Body = $@"
                    <h2>Подтверждение email</h2>
                    <p>Ваш код подтверждения: <strong>{code}</strong></p>
                    <p>Код действителен 15 минут.</p>
                    <p>Если вы не регистрировались, проигнорируйте это письмо.</p>"
            };

            try
            {
                await _emailService.SendEmailAsync(emailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
