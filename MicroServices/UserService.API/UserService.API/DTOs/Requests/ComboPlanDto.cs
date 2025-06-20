namespace UserService.API.DTOs.Requests
{
    public class ComboPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public Guid? LocationId { get; set; }
    }

}
