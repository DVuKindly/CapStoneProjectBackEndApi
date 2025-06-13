namespace MembershipService.API.Dtos.Response
{
    public class NextUServiceResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitType { get; set; }
        public Guid EcosystemId { get; set; }
        public string EcosystemName { get; set; }
    }
}
