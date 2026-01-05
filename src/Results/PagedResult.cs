namespace NDB.Abstraction.Results;
public sealed class PagedResult<T> : CollectionResult<T>
{
    public PageInfo PageInfo { get; init; } = PageInfo.Empty;

    private PagedResult() { }

    public static PagedResult<T> Ok(
        IReadOnlyList<T> items,
        int page,
        int pageSize,
        int totalItems,
        string message = "OK")
    {
        return new PagedResult<T>
        {
            Status = ResultStatus.Success,
            Items = items,
            Message = message,
            PageInfo = PageInfo.Create(page, pageSize, totalItems)
        };
    }
    public static new PagedResult<T> BadRequest(string message)
      => new()
      {
          Status = ResultStatus.BadRequest,
          PageInfo = PageInfo.Create(0,0,0),
          Message = message
      };

    public static new PagedResult<T> Unauthorized(string message)
      => new()
      {
          Status = ResultStatus.Unauthorized,
          PageInfo = PageInfo.Create(0, 0, 0),
          Message = message
      };

    public static new PagedResult<T> NotFound(string message)
      => new()
      {
          Status = ResultStatus.NotFound,
          PageInfo = PageInfo.Create(0, 0, 0),
          Message = message
      };

    public static new PagedResult<T> Error(string message)
      => new()
      {
          Status = ResultStatus.Error,
          PageInfo = PageInfo.Create(0, 0, 0),
          Message = message
      };

    public static new PagedResult<T> Fail(ResultStatus status, string message)
      => new()
      {
          Status = status,
          PageInfo = PageInfo.Create(0, 0, 0),
          Message = message
      };
}
