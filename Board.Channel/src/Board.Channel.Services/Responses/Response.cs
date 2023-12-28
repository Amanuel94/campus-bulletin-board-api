public class Response<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public object Data { get; set; } = null!;
    public List<string> Errors { get; set; } = null!;

    public static Response<T> Success(T data)
    {
        return new Response<T>
        {
            IsSuccess = true,
            Data = data!
        };
    }

    public static Response<T> Fail(string message, List<string> errors)
    {
        return new Response<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
}
