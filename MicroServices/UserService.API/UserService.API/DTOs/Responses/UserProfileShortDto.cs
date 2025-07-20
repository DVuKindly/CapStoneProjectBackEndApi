namespace UserService.API.DTOs.Responses
{
    public class UserProfileShortDto
    {
        public Guid AccountId { get; set; }
        public Guid? LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? RoleType { get; set; }

        public Guid? CityId { get; set; } 
    }
}
