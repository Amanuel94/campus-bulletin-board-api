namespace Board.Common.Responses;
public class Response<T>
{
    public  bool IsSuccess { get; set; }
    public  string Message { get; set; } = null!;
    public  T Data { get; set; } = default!;
    public List<string> Errors { get; set; } = new List<string>();

    public static Response<T> Success(T data)
    {

        return new Response<T>
        {
            IsSuccess = true,
            Data = data
        };

    }

    public static Response<T> Fail(string msg, List<string> errors)
    {

        return new Response<T>
        {
            IsSuccess = false,
            Message = msg,
            Errors = errors
        };

    }
}
