namespace AuthService.API.DTOs.Responses
{
    public class StatusResponse
    {
        public string? Email { get; set; }
        public bool IsVerified { get; set; }
        public bool IsLocked { get; set; }
    }
}
