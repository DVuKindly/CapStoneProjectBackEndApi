using AuthService.API.Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace AuthService.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;
        private const string ClientUrl = "http://localhost:3000";

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> options)
        {
            _logger = logger;
            _emailSettings = options.Value;
        }

        public async Task SendVerificationEmailAsync(string email, string verificationToken)
        {
            var link = $"{ClientUrl}/verify-email?token={verificationToken}";
            var subject = "Xác minh tài khoản NextU";
            var body = $"<p>Vui lòng nhấn vào liên kết sau để xác minh tài khoản:</p>" +
                       $"<p><a href='{link}'>{link}</a></p>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendResetPasswordEmailAsync(string email, string resetToken)
        {
            var link = $"{ClientUrl}/reset-password?token={resetToken}";
            var subject = "Khôi phục mật khẩu NextU";
            var body = $"<p>Nhấn vào liên kết sau để đặt lại mật khẩu:</p>" +
                       $"<p><a href='{link}'>{link}</a></p>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendSetPasswordEmailAsync(string email, string setPasswordToken)
        {
            var link = $"{ClientUrl}/set-password?token={setPasswordToken}";
            var subject = "Thiết lập mật khẩu tài khoản NextU";
            var body = $@"
                <p>Bạn đã được mời tham gia hệ thống NextU.</p>
                <p>Nhấn vào liên kết dưới đây để thiết lập mật khẩu:</p>
                <p><a href='{link}'>{link}</a></p>
                <p>Liên kết có hiệu lực trong 24 giờ.</p>";

            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.Username));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlContent
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation($"✅ Email sent to {toEmail} - Subject: {subject}");
        }
    }
}
