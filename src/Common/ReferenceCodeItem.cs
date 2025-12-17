namespace NDB.Abstraction.Common;

/// <summary>
/// Represents a reference item with identifier, business code, and display label.
/// Suitable for master data and enterprise lookups.
/// </summary>
public record ReferenceCodeItem<TId>(
    TId Id,
    string Code,
    string Label
);
