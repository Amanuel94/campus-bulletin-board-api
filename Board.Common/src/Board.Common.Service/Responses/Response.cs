// Purpose: This file contains the CommonResponse class, which is used to encapsulate the result of an operation.
namespace Board.Common.Response;

/// <summary>
/// Represents a common response object that is used to encapsulate the result of an operation.
/// </summary>
/// <typeparam name="T">The type of the data contained in the response.</typeparam>
public class CommonResponse<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the message associated with the response.
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Gets or sets the data contained in the response.
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// Gets or sets the list of errors associated with the response.
    /// </summary>
    public List<string> Errors { get; set; } = new List<string>();

    /// <summary>
    /// Creates a new instance of the <see cref="CommonResponse{T}"/> class representing a successful response.
    /// </summary>
    /// <param name="data">The data to be included in the response.</param>
    /// <returns>A new instance of the <see cref="CommonResponse{T}"/> class representing a successful response.</returns>
    public static CommonResponse<T> Success(T data)
    {
        return new CommonResponse<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CommonResponse{T}"/> class representing a failed response.
    /// </summary>
    /// <param name="msg">The message associated with the response.</param>
    /// <param name="errors">The list of errors associated with the response.</param>
    /// <returns>A new instance of the <see cref="CommonResponse{T}"/> class representing a failed response.</returns>
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
