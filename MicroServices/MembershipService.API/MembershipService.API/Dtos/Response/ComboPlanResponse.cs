namespace MembershipService.API.Dtos.Response
{
    public class ComboPlanResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public float DiscountRate { get; set; }
        public bool IsSuggested { get; set; }

        public Guid LocationId { get; set; }
        public string? LocationName { get; set; }

        public Guid PackageLevelId { get; set; }
        public string? PackageLevelName { get; set; }

        public Guid BasicPlanId { get; set; }
        public string? BasicPlanName { get; set; }

        public List<Guid> NextUServiceIds { get; set; }


    }
}
