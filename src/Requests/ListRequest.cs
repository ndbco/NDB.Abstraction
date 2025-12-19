namespace NDB.Abstraction.Requests;
public sealed class ListRequest
{
    public string? Search { get; init; }

    public IReadOnlyList<FilterRequest> Filters { get; init; }
        = Array.Empty<FilterRequest>();

    public IReadOnlyList<SortRequest> Sorts { get; init; }
        = Array.Empty<SortRequest>();

    public PagingRequest Paging { get; init; }
        = new();
}
