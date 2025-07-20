namespace UserService.API.DTOs.Responses
{
    public class ComboPlanResponse : IPlanResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }

        public string? Description { get; set; }

        public decimal TotalPrice { get; set; }

        public string? LocationName { get; set; }

        public Guid? LocationId { get; set; }
    }
}
