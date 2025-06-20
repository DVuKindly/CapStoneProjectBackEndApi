namespace AuthService.API.DTOs.Responses
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? IdToken { get; set; }

        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public Guid? UserId { get; set; }
        public Guid? LocationId { get; set; }

    }
}
