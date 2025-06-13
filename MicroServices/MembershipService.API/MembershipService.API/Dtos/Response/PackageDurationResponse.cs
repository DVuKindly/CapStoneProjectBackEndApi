namespace MembershipService.API.Dtos.Response
{
    public class PackageDurationResponse
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
