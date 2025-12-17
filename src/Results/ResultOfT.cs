
using NDB.Abstraction.Results;

public class Result<T> : Result
{
    public T? Data { get; init; }
    public static Result<T> Ok(T data, string message = "OK")
        => new()
        {
            Status = ResultStatus.Success,
            Data = data,
            Message = message
        };
}
