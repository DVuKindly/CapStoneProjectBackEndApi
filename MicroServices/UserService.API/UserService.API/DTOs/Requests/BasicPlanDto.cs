namespace UserService.API.DTOs.Requests
{
    public class BasicPlanDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty; 

        public decimal Price { get; set; }

        public Guid? LocationId { get; set; }

        public string LocationName { get; set; } = string.Empty;

        public int PackageDurationValue { get; set; }

        public string PackageDurationUnit { get; set; } = string.Empty;

        public bool VerifyBuy { get; set; } 
    }
}
