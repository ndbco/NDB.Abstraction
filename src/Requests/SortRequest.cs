namespace NDB.Abstraction.Requests;

public class SortRequest
{
    public string Field { get; set; } = default!;
    public SortDirection Direction { get; set; } = SortDirection.Asc;
}

public enum SortDirection
{
    Asc,
    Desc
}