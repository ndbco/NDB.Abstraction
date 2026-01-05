namespace NDB.Abstraction.Results;

public abstract class Result
{
    public ResultStatus Status { get; init; }
    public string Message { get; init; } = string.Empty;

    public bool Succeeded => Status == ResultStatus.Success;

    protected Result() { }

    public static Result Ok(string message = "OK")
        => new SimpleResult(ResultStatus.Success, message);

    public static Result BadRequest(string message)
        => new SimpleResult(ResultStatus.BadRequest, message);

    public static Result Unauthorized(string message)
        => new SimpleResult(ResultStatus.Unauthorized, message);

    public static Result NotFound(string message)
        => new SimpleResult(ResultStatus.NotFound, message);

    public static Result Error(string message)
        => new SimpleResult(ResultStatus.Error, message);

    public static Result Fail(ResultStatus status, string message)
        => new SimpleResult(status, message);

    private sealed class SimpleResult : Result
    {
        public SimpleResult(ResultStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
