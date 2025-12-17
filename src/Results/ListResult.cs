namespace NDB.Abstraction.Results;

public class ListResult<T> : Result
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int TotalCount { get; init; }
}
