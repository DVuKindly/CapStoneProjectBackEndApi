namespace BffService.API.DTOs.Responses
{
    public class UserProfileResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserProfileDto? Data { get; set; }
    }
}
