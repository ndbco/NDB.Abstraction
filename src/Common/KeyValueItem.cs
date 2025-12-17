namespace NDB.Abstraction.Common;

/// <summary>
/// Represents a simple key-value pair.
/// Useful for status mappings, configuration, or simple lookups.
/// </summary>
public record KeyValueItem<TKey>(
    TKey Key,
    string Value
);
