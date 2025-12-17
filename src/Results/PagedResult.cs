namespace NDB.Abstraction.Results;

public class PagedResult<T> : Result
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public PageInfo PageInfo { get; init; } = default!;
}

public class PageInfo
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }

    public int TotalPages =>
        (int)Math.Ceiling((double)TotalItems / PageSize);
}
