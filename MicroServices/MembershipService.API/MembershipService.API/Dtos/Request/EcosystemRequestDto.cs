namespace MembershipService.API.Dtos.Request
{
    public class EcosystemRequestDto
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
