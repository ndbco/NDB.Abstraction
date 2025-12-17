namespace NDB.Abstraction.Requests;

public class FilterRequest
{
    public string Field { get; set; } = default!;
    public string Value { get; set; } = default!;
    public FilterOperator Operator { get; set; } = FilterOperator.Contains;
}

public enum FilterOperator
{
    Equals,
    Contains,
    StartsWith,
    EndsWith,
    GreaterThan,
    LessThan
}
