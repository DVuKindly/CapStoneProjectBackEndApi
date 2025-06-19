namespace UserService.API.DTOs.Requests
{
    public class BasicPlanDto
    {
        public Guid Id { get; set; }
        public string LocationName { get; set; }
        public decimal Price { get; set; }
        public Guid? LocationId { get; set; }
        public string? Duration { get; set; }
        public string? Description { get; set; }
    }
}
