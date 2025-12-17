namespace NDB.Abstraction.Results;

public class Result
{
    public ResultStatus Status { get; init; }
    public string Message { get; init; } = string.Empty;

    public bool Succeeded => Status == ResultStatus.Success;

    public static Result Ok(string message = "OK")
        => new() { Status = ResultStatus.Success, Message = message };

    public static Result Fail(ResultStatus status, string message)
        => new() { Status = status, Message = message };
}
