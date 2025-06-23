namespace AuthService.API.DTOs.Request
{
    public class ChangeRoleRequest
    {
        public Guid UserId { get; set; }
        public Guid NewRoleId { get; set; }
        public string? Reason { get; set; }
    }

}
