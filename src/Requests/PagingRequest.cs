namespace NDB.Abstraction.Requests;

public sealed class PagingRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    public int Skip =>
        (Page <= 1 ? 0 : (Page - 1) * PageSize);

    public int Take =>
        PageSize <= 0 ? 20 : PageSize;
}