namespace BffService.API.DTOs.Responses
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Message { get; set; }
    }
}
