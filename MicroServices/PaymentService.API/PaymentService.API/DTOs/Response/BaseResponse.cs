namespace PaymentService.API.DTOs.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        // ✅ Helper để tạo response thất bại
        public static BaseResponse Fail(string message)
        {
            return new BaseResponse
            {
                Success = false,
                Message = message
            };
        }

        // ✅ Helper để tạo response thành công
        public static BaseResponse Ok(object? data = null, string message = "Thành công")
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
