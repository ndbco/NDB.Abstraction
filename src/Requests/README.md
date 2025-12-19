# NDB.Abstraction – Requests Guide (with Examples)

This document defines the **standard request models** used in
**NDB.Abstraction.Requests** and demonstrates **real-world usage examples**.

Request models describe **WHAT the client wants**,
not **HOW the query is executed**.

---

## PURPOSE OF REQUEST MODELS

### Request models ARE used to:

* Represent query intent (search, filter, sort, paging)
* Standardize list-based requests
* Enable reusable query pipelines
* Keep handlers thin and predictable

### Request models are NOT used to:

* Execute EF or LINQ queries
* Contain business logic
* Perform authorization
* Build dynamic SQL or string-based queries

---

## CORE DESIGN RULES

> **Request models are immutable input contracts.**

* Properties use `init`
* Defaults are safe
* No exceptions are thrown
* Validation is external (FluentValidation / pipeline)

---

## LISTREQUEST (ROOT QUERY REQUEST)

```csharp
public sealed class ListRequest
{
    public string? Search { get; init; }

    public IReadOnlyList<FilterRequest> Filters { get; init; }
        = Array.Empty<FilterRequest>();

    public IReadOnlyList<SortRequest> Sorts { get; init; }
        = Array.Empty<SortRequest>();

    public PagingRequest Paging { get; init; }
        = new();
}
```

### Typical JSON payload

```json
{
  "search": "admin",
  "filters": [
    { "field": "Active", "value": "true", "operator": "Equals" }
  ],
  "sorts": [
    { "field": "Name", "direction": "Asc" }
  ],
  "paging": {
    "page": 1,
    "pageSize": 10
  }
}
```

---

## GLOBAL SEARCH

### Purpose

* Performs keyword search across multiple fields
* Fields are defined by **whitelist** in application layer

### Example

```json
{
  "search": "administrator"
}
```

---

## FILTERREQUEST

```csharp
public sealed class FilterRequest
{
    public string Field { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public FilterOperator Operator { get; init; }
        = FilterOperator.Contains;
}
```

### FilterOperator values

* `Equals`
* `Contains`
* `StartsWith`
* `EndsWith`
* `GreaterThan`
* `LessThan`

### Example

```json
{
  "filters": [
    { "field": "Code", "value": "ADM", "operator": "StartsWith" }
  ]
}
```

---

## SORTREQUEST

```csharp
public sealed class SortRequest
{
    public string Field { get; init; } = string.Empty;
    public SortDirection Direction { get; init; }
        = SortDirection.Asc;
}
```

### SortDirection values

* `Asc`
* `Desc`

### Example

```json
{
  "sorts": [
    { "field": "CreatedDate", "direction": "Desc" }
  ]
}
```

---

## PAGINGREQUEST

```csharp
public sealed class PagingRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
```

### Example

```json
{
  "paging": {
    "page": 2,
    "pageSize": 25
  }
}
```

---

## END-TO-END QUERY EXAMPLE

### 1️⃣ Client Request (JSON)

```json
{
  "search": "admin",
  "filters": [
    { "field": "Active", "value": "true", "operator": "Equals" }
  ],
  "sorts": [
    { "field": "Name", "direction": "Asc" }
  ],
  "paging": {
    "page": 1,
    "pageSize": 10
  }
}
```

---

### 2️⃣ Handler Usage

```csharp
public async Task<Result> Handle(
    GetRoleListRequest request,
    CancellationToken ct)
{
    return await _db.ROLE
        .ReadOnly()
        .ApplySearch(
            request.Search,
            RoleQueryMap.SearchableFields)
        .ApplyFilters(
            request.Filters,
            RoleQueryMap.AllowedFilters)
        .ApplySorts(
            request.Sorts,
            RoleQueryMap.AllowedSorts)
        .ToPagedResultAsync<Role, RoleResponse>(
            request.Paging,
            _mapper,
            ct);
}
```

---

### 3️⃣ Response (PagedResult)

```json
{
  "status": "Success",
  "message": "OK",
  "items": [
    {
      "id": "1",
      "code": "ADM",
      "name": "Administrator"
    }
  ],
  "pageInfo": {
    "page": 1,
    "pageSize": 10,
    "totalItems": 1,
    "totalPages": 1
  }
}
```

---

## VALIDATION STRATEGY

Validation is handled **outside request models**.

### Example (FluentValidation)

```csharp
RuleFor(x => x.Paging.Page).GreaterThan(0);
RuleFor(x => x.Paging.PageSize).LessThanOrEqualTo(100);
```

Invalid requests:

* Do not execute handlers
* Return ValidationResult early

---

## SECURITY CONSIDERATIONS

* Field names are validated via whitelist
* No dynamic SQL or LINQ execution
* Expression trees are used for queries
* Invalid filters or sorts are ignored safely

---

## ANTI-PATTERNS (DO NOT USE)

* ❌ Executing EF queries inside Request
* ❌ Using dynamic string-based LINQ
* ❌ Trusting client-provided field names blindly
* ❌ Mutating Request objects

---

## DESIGN PRINCIPLES

* Requests represent **intent**
* Queries are explicit and controlled
* Handlers remain thin
* Abstractions are stable and reusable

---

## SUMMARY

### Use:

* `ListRequest` for list queries
* `Search` for keyword search
* `FilterRequest` for structured filtering
* `SortRequest` for ordering
* `PagingRequest` for pagination

### Avoid:

* Mixing request and query logic
* Dynamic string-based queries
* Mutable request objects

---

This document defines the **official request contract**
for **NDB.Abstraction.Requests** and ensures consistent,
secure, and scalable query handling across the system.
