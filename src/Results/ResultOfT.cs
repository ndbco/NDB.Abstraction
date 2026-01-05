
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
    public static new Result<T> BadRequest(string message)
      => new()
      {
          Status = ResultStatus.BadRequest,
          Message = message
      };

    public static new Result<T> Unauthorized(string message)
      => new()
      {
          Status = ResultStatus.Unauthorized,
          Message = message
      };

    public static new Result<T> NotFound(string message)
      => new()
      {
          Status = ResultStatus.NotFound,
          Message = message
      };

    public static new Result<T> Error(string message)
      => new()
      {
          Status = ResultStatus.Error,
          Message = message
      };

    public static new Result<T> Fail(ResultStatus status, string message)
      => new()
      {
          Status = status,
          Message = message
      };
}
