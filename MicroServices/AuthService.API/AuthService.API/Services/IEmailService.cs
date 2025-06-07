namespace AuthService.API.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verificationToken);
        Task SendResetPasswordEmailAsync(string email, string resetToken);
        Task SendSetPasswordEmailAsync(string email, string setPasswordToken); // ✅ Gửi link thiết lập mật khẩu
    }
}
