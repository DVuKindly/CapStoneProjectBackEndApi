namespace AuthService.API.DTOs.Responses
{
    public class UserProfileLiteDto
    {
        public Guid AccountId { get; set; }
        public Guid? LocationId { get; set; }
        public string? FullName { get; set; }
        public string LocationName { get; set; } = string.Empty; 
    }

}
