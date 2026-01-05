namespace NDB.Abstraction.Results;

public sealed class ListResult<T> : CollectionResult<T>
{
    public int TotalCount { get; init; }

    private ListResult() { }

    public static ListResult<T> Ok(
        IReadOnlyList<T> items,
        int? totalCount = null,
        string message = "OK")
    {
        return new ListResult<T>
        {
            Status = ResultStatus.Success,
            Items = items,
            TotalCount = totalCount ?? items.Count,
            Message = message
        };
    }
    public static new ListResult<T> BadRequest(string message)
      => new()
      {
          Status = ResultStatus.BadRequest,
          TotalCount = 0,
          Message = message
      };

    public static new ListResult<T> Unauthorized(string message)
      => new()
      {
          Status = ResultStatus.Unauthorized,
          TotalCount = 0,
          Message = message
      };

    public static new ListResult<T> NotFound(string message)
      => new()
      {
          Status = ResultStatus.NotFound,
          TotalCount = 0,
          Message = message
      };

    public static new ListResult<T> Error(string message)
      => new()
      {
          Status = ResultStatus.Error,
          TotalCount = 0,
          Message = message
      };

    public static new ListResult<T> Fail(ResultStatus status, string message)
      => new()
      {
          Status = status,
          TotalCount = 0,
          Message = message
      };
}
