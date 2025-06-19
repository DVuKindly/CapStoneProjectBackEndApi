namespace UserService.API.DTOs.Responses
{
    public class BasicPlanResponse
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public bool VerifyBuy { get; set; }

        public int PackageDurationId { get; set; }

        public string? PackageDurationName { get; set; }

        public Guid LocationId { get; set; }

        public string? LocationName { get; set; }

        public List<Guid> NextUServiceIds { get; set; } = new(); // Tránh null exception khi deserialize
    }
}
