namespace NDB.Abstraction.Requests;

public sealed class FilterRequest
{
    public string Field { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public FilterOperator Operator { get; init; } = FilterOperator.Contains;
    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Field) &&
        !string.IsNullOrWhiteSpace(Value);
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
