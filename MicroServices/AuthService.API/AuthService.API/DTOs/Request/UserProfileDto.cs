namespace AuthService.API.DTOs.Request
{
    public class UserProfileDto
    {
        public Guid AccountId { get; set; }
        public string RoleType { get; set; } = string.Empty;
        public Guid LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
    }
}
