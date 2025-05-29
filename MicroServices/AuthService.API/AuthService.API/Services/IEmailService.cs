namespace AuthService.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendVerificationEmailAsync(string email, string verificationToken)
        {
            var link = $"https://yourdomain.com/verify-email?token={verificationToken}";
            _logger.LogInformation($"[Email] Send verification to {email}: {link}");
            return Task.CompletedTask;
        }

        public Task SendResetPasswordEmailAsync(string email, string resetToken)
        {
            var link = $"https://yourdomain.com/reset-password?token={resetToken}";
            _logger.LogInformation($"[Email] Send reset password to {email}: {link}");
            return Task.CompletedTask;
        }
    }
}