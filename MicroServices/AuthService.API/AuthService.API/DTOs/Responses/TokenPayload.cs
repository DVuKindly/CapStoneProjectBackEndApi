namespace AuthService.API.DTOs.Responses
{
    public class TokenPayload
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
