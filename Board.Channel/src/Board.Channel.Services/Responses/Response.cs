public class CommonResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public object Data { get; set; } = null!;
    public List<string> Errors { get; set; } = null!;

    public static CommonResponse<T> Success(T data)
    {
        return new CommonResponse<T>
        {
            IsSuccess = true,
            Data = data!
        };
    }

    public static CommonResponse<T> Fail(string message, List<string> errors)
    {
        return new CommonResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
}
