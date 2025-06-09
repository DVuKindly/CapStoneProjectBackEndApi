namespace UserService.API.DTOs.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
