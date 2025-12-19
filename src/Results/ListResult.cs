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
}
