# NDB.Abstraction – Results Guide

This document defines the **official, enforced pattern** for using `Result` models
in **NDB.Abstraction**.

The goal is to ensure all backend responses are:

* **Explicit**
* **Immutable**
* **Predictable**
* **Misuse-resistant**

Result models describe **WHAT happened**, not **HOW it is transported**.

---

## PURPOSE OF RESULT MODELS

### Result models ARE used to:

* Standardize backend outcomes
* Represent success or failure explicitly
* Separate business results from transport (HTTP, gRPC, etc.)
* Provide consistent response shapes for clients

### Result models are NOT used to:

* Replace HTTP status codes
* Perform logging or monitoring
* Contain business logic
* Swallow or replace exceptions

---

## CORE DESIGN RULES (IMPORTANT)

> **All Result types are created ONLY via factory methods.**

### This means:

* ❌ `new Result()`
* ❌ `new Result<T>()`
* ❌ `new ListResult<T>()`
* ❌ `new PagedResult<T>()`

are **NOT allowed**.

This is intentional and enforced to prevent:

* silent success
* missing status
* inconsistent state

---

## BASE RESULT TYPE

All result types inherit from the same base abstraction.

### Base properties:

* `Status` – semantic outcome of the operation
* `Message` – human-readable message
* `Succeeded` – `true` if `Status == Success`

### ResultStatus values:

* `Success`
* `BadRequest`
* `NotFound`
* `Unauthorized`
* `Forbidden`
* `Conflict`
* `Error`

`ResultStatus` is **transport-agnostic** and can be mapped to HTTP, gRPC, or others.

---

## RESULT (NON-GENERIC)

Use **Result (non-generic)** when:

* The operation does NOT return data
* The operation represents a command (create, update, delete, activate)

### SUCCESS

```csharp
return Result.Ok("Role activated successfully");
```

### FAILURE

```csharp
return Result.Fail(
    ResultStatus.NotFound,
    "Role not found");
```

### Internal note

`Result.Ok()` and `Result.Fail()` return an internal **SimpleResult**
implementation.
`SimpleResult` is **not accessible** and **cannot be instantiated directly**.

---

## RESULT<T> (GENERIC RESULT)

### Use Result<T> ONLY when:

* Returning data
* The operation is successful

### RULE (STRICT)

> **Result<T> is ONLY for success cases.**

Failure cases must return **Result**, not `Result<T>`.

### SUCCESS WITH DATA

```csharp
return Result<RoleResponse>.Ok(role);
```

### ❌ BAD PRACTICE (DO NOT DO THIS)

```csharp
return new Result<RoleResponse>
{
    Status = ResultStatus.BadRequest,
    Data = null
};
```

---

## COLLECTION RESULTS

Collection results represent **lists of data**.
They are immutable and factory-only.

---

## LISTRESULT<T>

### Use ListResult<T> when:

* Returning a collection
* Paging metadata is NOT required

### Properties:

* `Items` – read-only list of data
* `TotalCount` – total number of items

### SUCCESS

```csharp
return ListResult<RoleResponse>.Ok(items);
```

### Typical use cases:

* Dropdown lists
* Master data
* Simple list endpoints

---

## PAGEDRESULT<T>

### Use PagedResult<T> when:

* Returning a collection WITH paging metadata
* The UI requires paging information

### Properties:

* `Items` – read-only list of data
* `PageInfo` – paging metadata

### PageInfo properties:

* `Page` – current page
* `PageSize` – items per page
* `TotalItems` – total data count
* `TotalPages` – computed safely

### SUCCESS

```csharp
return PagedResult<RoleResponse>.Ok(
    items,
    page,
    pageSize,
    totalItems);
```

### Default behavior

If paging information is not explicitly set:

* `PageInfo.Empty` is used
* No `null` values are exposed

---

## PAGEINFO SAFETY

`PageInfo` is immutable and safe by default.

* No `null`
* No `default!`
* No divide-by-zero

```csharp
public static PageInfo.Empty
```

is used as a safe fallback.

---

## VALIDATIONRESULT (if applicable)

`ValidationResult` is a **specialized Result type** for input validation errors.

### Use ValidationResult ONLY for:

* Field-level validation
* Input errors

### ValidationResult contains:

* `Status` (usually `BadRequest`)
* `Errors` – structured field errors

### EXAMPLE

```csharp
return new ValidationResult(
    new[]
    {
        new ValidationError("Name", "Name is required"),
        new ValidationError("Age", "Age must be greater than zero")
    });
```

### RULES

* ValidationResult is NOT generic
* ValidationResult replaces Result<T> for invalid input
* ValidationResult does NOT represent business errors

---

## END-TO-END HANDLER EXAMPLE

```csharp
public async Task<Result> Handle(GetRolesQuery request)
{
    var roles = await _db.Roles.ToListAsync();

    var items = _mapper.Map<List<RoleResponse>>(roles);

    return ListResult<RoleResponse>.Ok(items);
}
```

---

## TRANSPORT / CONTROLLER MAPPING

Result models are **transport-independent**.

Example HTTP mapping:

```text
Success      -> 200 OK
BadRequest   -> 400
NotFound     -> 404
Unauthorized -> 401
Forbidden    -> 403
Conflict     -> 409
Error        -> 500
```

Controllers should map **ResultStatus only**,
not inspect Result types.

---

## ANTI-PATTERNS (DO NOT USE)

* ❌ `new Result()`
* ❌ Returning `Result<T>` for failures
* ❌ Mutating Result after creation
* ❌ Using exceptions for expected validation failures
* ❌ AutoMapper mapping Result objects

---

## DESIGN PRINCIPLES

* Result models describe **outcomes**, not flow
* Explicit factories prevent silent bugs
* Immutable responses are safer for teams
* Transport concerns are always external

---

## SUMMARY

### Use:

* `Result` → commands without data
* `Result<T>` → successful operations with data
* `ListResult<T>` → collections without paging
* `PagedResult<T>` → collections with paging
* `ValidationResult` → structured validation errors

### Avoid:

* Instantiating Result types directly
* Using generic results for failure cases
* Mixing business logic into Result models

---

This document defines the **official contract** for result handling in
**NDB.Abstraction** and must be followed to ensure consistency,
clarity, and long-term maintainability.