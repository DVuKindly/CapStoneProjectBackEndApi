namespace AuthService.API.DTOs.Request
{
    public class CreateUserProfileRequest
    {
        public string AccountId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
