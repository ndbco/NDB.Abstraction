namespace NDB.Abstraction.Common;

/// <summary>
/// Represents a generic lookup item for UI selections.
/// Can be used for dropdowns, radio buttons, and selects.
/// </summary>
public record LookupItem<T>(
    T Value,
    string Label,
    bool Disabled = false
);
