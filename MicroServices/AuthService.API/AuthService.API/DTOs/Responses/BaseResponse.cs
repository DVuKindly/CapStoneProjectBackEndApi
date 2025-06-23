namespace AuthService.API.DTOs.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public BaseResponse() { }

        public BaseResponse(bool success, string? message)
        {
            Success = success;
            Message = message;
        }
    }
}
