namespace UserService.API.DTOs.Requests
{
    public class ApprovePendingMembershipRequest
    {
        public Guid AccountId { get; set; }
        public string ApprovedBy { get; set; } = null!;
        public string StaffNote { get; set; } = null!;
    }

}
