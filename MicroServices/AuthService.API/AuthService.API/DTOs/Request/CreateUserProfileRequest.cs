namespace AuthService.API.DTOs.Request
{
    public class CreateUserProfileRequest
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
