namespace UserService.API.DTOs.Requests
{
    public class CreateCityDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

}
