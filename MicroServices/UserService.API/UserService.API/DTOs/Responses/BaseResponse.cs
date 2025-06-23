namespace UserService.API.DTOs.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        // ✅ Method Fail
        public static BaseResponse Fail(string message)
        {
            return new BaseResponse
            {
                Success = false,
                Message = message
            };
        }

        // ✅ Method Ok (KHÔNG đặt tên là Success)
        public static BaseResponse Ok(string message, object? data = null)
        {
            return new BaseResponse
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
    }



}
