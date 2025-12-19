
using NDB.Abstraction.Results;
public sealed class Result<T> : Result
{
    public T? Data { get; init; }

    private Result() { }

    public static Result<T> Ok(T data, string message = "OK")
        => new()
        {
            Status = ResultStatus.Success,
            Data = data,
            Message = message
        };
}
