namespace AuthService.API.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verificationToken);
        Task SendResetPasswordEmailAsync(string email, string resetToken);
    }
}
