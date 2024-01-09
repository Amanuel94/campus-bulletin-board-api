namespace Board.Common.Response;
public class CommonResponse<T>
{
    public  bool IsSuccess { get; set; }
    public  string Message { get; set; } = null!;
    public  T Data { get; set; } = default!;
    public List<string> Errors { get; set; } = new List<string>();

    public static CommonResponse<T> Success(T data)
    {

        return new CommonResponse<T>
        {
            IsSuccess = true,
            Data = data
        };

    }

    public static CommonResponse<T> Fail(string msg, List<string> errors)
    {

        return new CommonResponse<T>
        {
            IsSuccess = false,
            Message = msg,
            Errors = errors
        };

    }
}
