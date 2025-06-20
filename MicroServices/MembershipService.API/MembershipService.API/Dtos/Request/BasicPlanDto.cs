namespace MembershipService.API.Dtos.Request
{
    public class BasicPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }  
    }
}
