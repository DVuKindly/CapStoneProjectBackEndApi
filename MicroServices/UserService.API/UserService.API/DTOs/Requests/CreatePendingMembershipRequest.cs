namespace UserService.API.DTOs.Requests
{
    public class CreatePendingMembershipRequest
    {
        public Guid AccountId { get; set; }
        public Guid PackageId { get; set; }
        public string RequestedPackageName { get; set; } = null!;
        public string Location { get; set; } = null!;
    }

}
