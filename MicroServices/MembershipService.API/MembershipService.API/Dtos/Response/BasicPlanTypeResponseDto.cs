namespace MembershipService.API.Dtos.Response
{
    public class BasicPlanTypeResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
