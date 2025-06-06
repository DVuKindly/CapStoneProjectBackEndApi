namespace UserService.API.DTOs.Requests
{
    public class RejectPendingMembershipRequest
    {
        public Guid AccountId { get; set; }
        public string StaffNote { get; set; } = null!;
    }

}
