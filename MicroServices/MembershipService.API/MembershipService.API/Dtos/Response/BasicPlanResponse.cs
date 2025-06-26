namespace MembershipService.API.Dtos.Response
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
        public string? DurationDescription { get; set; }

        public List<Guid> NextUServiceIds { get; set; } = new();


      
        public int PackageDurationValue { get; set; }

       
        public string PackageDurationUnit { get; set; } = string.Empty;

      
        public string PlanSource { get; set; } = "basic";

    }
}
