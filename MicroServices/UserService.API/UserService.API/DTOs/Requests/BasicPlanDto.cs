namespace UserService.API.DTOs.Requests
{
    public class BasicPlanDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }      // ✅ dùng được plan.Name
        public decimal Price { get; set; }
        public Guid? LocationId { get; set; }
        public string? LocationName { get; set; }

        public bool VerifyBuy { get; set; }    // ✅ cần để xử lý logic mua trực tiếp
    }

}
