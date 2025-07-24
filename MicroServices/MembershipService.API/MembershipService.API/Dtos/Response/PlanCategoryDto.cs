namespace MembershipService.API.Dtos.Response
{
    public class PlanCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}