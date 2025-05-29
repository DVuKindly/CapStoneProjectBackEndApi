namespace BffService.API.DTOs.Auth.Request
{
    public class GoogleLoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ProviderId { get; set; } = string.Empty;
    }
}
