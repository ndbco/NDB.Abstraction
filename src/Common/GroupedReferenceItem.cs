namespace NDB.Abstraction.Common;

/// <summary>
/// Represents a reference item grouped under a specific category.
/// Useful for grouped dropdowns or categorized selections.
/// </summary>
public record GroupedReferenceItem<T>(
    T Value,
    string Label,
    string Group
);
