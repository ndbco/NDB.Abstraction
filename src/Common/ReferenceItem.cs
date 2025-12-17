namespace NDB.Abstraction.Common;

/// <summary>
/// Represents a lightweight reference item with identifier and display label.
/// Commonly used for lookup and dropdown data.
/// </summary>
public record ReferenceItem<TId>(
    TId Id,
    string Label
);
