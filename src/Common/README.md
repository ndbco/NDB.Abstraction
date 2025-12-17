# NDB.Abstraction – Common Contracts

This document explains the purpose and usage of **Common contracts**
provided by **NDB.Abstraction**.

Common contracts are **lightweight value objects** designed to standardize
frequently used data shapes across APIs, services, and UI applications.

They are intentionally simple, immutable, and framework-agnostic.

---

## Purpose of Common Contracts

Common contracts are used to:

- Standardize frequently repeated DTO shapes
- Reduce duplicated “lookup” or “reference” models
- Provide consistent payloads for UI components
- Improve readability and maintainability of APIs

Common contracts are **NOT** used to:

- Contain business logic
- Replace domain entities or aggregates
- Perform transformations or calculations
- Act as helper or utility classes

---

## Design Philosophy

All common contracts follow these principles:

- **Contracts only** — no behavior
- **UI-friendly but not UI-specific**
- **Domain-agnostic**
- **Safe for long-term public APIs**
- **Minimal and explicit**

They represent **reference data**, not business models.

---

## Available Common Contracts

The following common contracts are available in `NDB.Abstraction.Common`:

- `ReferenceItem<TId>`
- `ReferenceCodeItem<TId>`
- `LookupItem<T>`
- `KeyValueItem<TKey>`
- `GroupedReferenceItem<T>`

---

## ReferenceItem<TId>

Represents a lightweight reference with an identifier and display label.

### Structure
- `Id`    : identifier (generic)
- `Label` : display label

### Example

```code
using NDB.Abstraction.Common;

var plant = new ReferenceItem<Guid>(
    id: Guid.NewGuid(),
    label: "Plant Jakarta"
);
```

## Typical Use Cases
- Dropdown lists
- Simple lookup APIs
- Reference data

### ReferenceCodeItem<TId>
Represents a reference with identifier, business code, and label.
Structure : 
- Id : identifier
- Code : business or short code
- Label : display label


### Example
```code
var vehicleType = new ReferenceCodeItem<Guid>(
    id: Guid.NewGuid(),
    code: "SUV",
    label: "Sport Utility Vehicle"
);
```

### Typical Use Cases : 
- Master data
- Enterprise lookup APIs
- UI selections that require both code and label

### LookupItem<T>
Represents a generic lookup option for UI selections.

### Structure : 
- Value : underlying value
- Label : display label
- Disabled : whether the option is selectable

### Example
```code
var statusOptions = new List<LookupItem<int>>
{
    new(1, "Active"),
    new(0, "Inactive", disabled: true)
};
```
### Typical Use Cases : 
- Dropdowns
- Radio buttons
- Enum-to-label mappings

### KeyValueItem<TKey>

Represents a simple key–value pair.
### Structure :
- Key : identifier or key
- Value : display or mapped value

### Example
```code
var statusMapping = new KeyValueItem<string>(
    key: "ORDER",
    value: "Order Created"
);
```

### Typical Use Cases :
- Status mappings
- Configuration output
- Simple dictionary-like responses

### GroupedReferenceItem<T>
Represents a reference item grouped under a category.

### Structure
- Value : underlying value
- Label : display label
- Group : grouping category

### Example
```code
var cities = new List<GroupedReferenceItem<Guid>>
{
    new(Guid.NewGuid(), "Jakarta", "Indonesia"),
    new(Guid.NewGuid(), "Bandung", "Indonesia"),
    new(Guid.NewGuid(), "Tokyo", "Japan")
};
```
### Typical Use Cases :
- Grouped dropdowns
- Categorized selections
- Hierarchical reference data

## Returning Common Contracts in Results
Common contracts are typically returned inside result types.
### Example
```code
return Result.Ok(
    new ReferenceCodeItem<Guid>(
        id: vehicle.Id,
        code: vehicle.Code,
        label: vehicle.Name
    )
);
```
### Collection Example
```code
return Result.Ok(
    vehicleTypes.Select(v =>
        new ReferenceCodeItem<Guid>(v.Id, v.Code, v.Name)
    ).ToList()
);
```
## Anti-Patterns
Do NOT:
- Add logic or methods to common contracts
- Extend them for business behavior
- Treat them as domain models
- Create domain-specific “common” types

## Summary
Common contracts in NDB.Abstraction:
- Represent reusable reference data
- Improve consistency across services and UIs
- Reduce DTO duplication
- Are optional but highly practical

They form a shared vocabulary for small, generic data structures
without introducing coupling or complexity.