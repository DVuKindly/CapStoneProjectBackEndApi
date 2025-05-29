namespace BffService.API.DTOs.Auth.Response
{
    public class AuthStatusResponse
    {
        public bool Exists { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsLocked { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
