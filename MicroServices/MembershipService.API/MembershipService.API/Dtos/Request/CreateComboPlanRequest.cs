namespace MembershipService.API.Dtos.Request
{
    public class CreateComboPlanRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsSuggested { get; set; }
        public bool VerifyBuy { get; set; }
        public Guid? LocationId { get; set; }
        public Guid PackageLevelId { get; set; }

        public List<Guid> BasicPlanIds { get; set; } = new();
        public List<PackageDurationDto> PackageDurations { get; set; } = new();
    }

    public class UpdateComboPlanRequest : CreateComboPlanRequest { }
}
