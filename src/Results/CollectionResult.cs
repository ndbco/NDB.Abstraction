namespace NDB.Abstraction.Results;

public abstract class CollectionResult<T> : Result
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

    protected CollectionResult() { }
}