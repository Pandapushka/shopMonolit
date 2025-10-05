namespace shop.Services.EmailConfirmationService
{
    public interface IEmailConfirmationService
    {
        Task<string> GenerateConfirmationCodeAsync(string email);
        Task<bool> VerifyConfirmationCodeAsync(string email, string code);
        Task<bool> SendConfirmationCodeAsync(string email, string code);
    }
}
