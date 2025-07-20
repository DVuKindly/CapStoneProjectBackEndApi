public class BaseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }

    public static BaseResponse Fail(string message)
    {
        return new BaseResponse
        {
            Success = false,
            Message = message
        };
    }

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

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static BaseResponse<T> Fail(string message)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }

    public static BaseResponse<T> Ok(T data, string message = "")
    {
        return new BaseResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }
}

