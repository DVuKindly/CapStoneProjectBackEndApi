namespace AuthService.API.DTOs.Request
{
    public class UserLoginHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string? IPAddress { get; set; }
        public string? Device { get; set; }
    }
}
