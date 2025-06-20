namespace MembershipService.API.Dtos.Request
{
    public class ComboPlanCreateRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public float DiscountRate { get; set; }
        public bool IsSuggested { get; set; }

        public Guid LocationId { get; set; }
        public Guid PackageLevelId { get; set; }
        public Guid BasicPlanId { get; set; }

        public List<Guid> NextUServiceIds { get; set; }
    }

    public class ComboPlanUpdateRequest : ComboPlanCreateRequest { }
}
