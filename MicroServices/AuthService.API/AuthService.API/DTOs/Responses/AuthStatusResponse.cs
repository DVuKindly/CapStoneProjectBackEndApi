namespace AuthService.API.DTOs.Responses
{
    public class AuthStatusResponse
    {
        public bool Exists { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsLocked { get; set; }
        public string? FullName { get; set; }
    }
}
