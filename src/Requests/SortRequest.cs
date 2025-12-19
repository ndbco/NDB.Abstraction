namespace NDB.Abstraction.Requests;

public sealed class SortRequest
{
    public string Field { get; init; } = string.Empty;
    public SortDirection Direction { get; init; } = SortDirection.Asc;

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Field);
}

public enum SortDirection
{
    Asc,
    Desc
}