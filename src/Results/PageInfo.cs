namespace NDB.Abstraction.Results;
public sealed class PageInfo
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }

    public int TotalPages =>
        PageSize <= 0 ? 0 :
        (int)Math.Ceiling((double)TotalItems / PageSize);

    private PageInfo() { }

    public static PageInfo Empty { get; } = new()
    {
        Page = 1,
        PageSize = 0,
        TotalItems = 0
    };

    public static PageInfo Create(
        int page,
        int pageSize,
        int totalItems)
    {
        return new PageInfo
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
}

