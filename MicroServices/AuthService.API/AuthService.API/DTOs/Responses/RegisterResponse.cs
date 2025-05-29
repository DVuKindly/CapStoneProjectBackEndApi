namespace AuthService.API.DTOs.Responses
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? VerificationToken { get; set; }
        public string? Message { get; set; }
    }
}
