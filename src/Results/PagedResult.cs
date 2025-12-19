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
}
