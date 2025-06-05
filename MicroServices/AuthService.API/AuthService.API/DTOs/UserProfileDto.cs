namespace AuthService.API.DTOs
{
    public class UserProfileDto
    {
        public string AccountId { get; set; } = null!;
        public string RoleType { get; set; } = "User";
    }
}
