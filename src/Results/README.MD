# NDB.Abstraction – Results Guide

This document explains how RESULT models in NDB.Abstraction should be used
in a clear, consistent, and predictable way, including practical examples.
Result models define HOW the backend communicates outcomes
(success, failure, validation errors) to clients.

## PURPOSE OF RESULT MODELS

### Result models are used to:
- Standardize responses across services and APIs
- Clearly represent success or failure
- Separate business outcomes from transport protocols (HTTP, gRPC, etc.)
- Provide predictable response structures for UI and client applications

### Result models are NOT used to:
- Replace HTTP status codes
- Perform logging or monitoring
- Contain business logic
- Throw exceptions for expected cases

## BASE RESULT TYPE
All result types inherit from the same base type.

### Base Result properties:
- Status : semantic outcome of the operation
- Message : human-readable message
- Succeeded : true if Status == Success

### ResultStatus values:
- Success
- BadRequest
- NotFound
- Unauthorized
- Forbidden
- Conflict
- Error

ResultStatus is protocol-agnostic and can be mapped to HTTP,
gRPC, or any other transport layer.

## RESULT (NON-GENERIC)

Use Result (non-generic) when:
- The operation does not return data
- The operation represents a command (create, update, delete)

### EXAMPLE (SUCCESS):

```code
return Result.Ok("Vehicle created successfully");
```

### EXAMPLE (FAILURE):

```code
return Result.Fail(
    ResultStatus.Conflict,
    "Vehicle already exists"
);
```

## RESULT<T> (GENERIC RESULT)

### Use Result<T> when:
- Returning data on success
- Executing read or query operations

### IMPORTANT RULE:
- Result<T> is ONLY for SUCCESS cases
- Validation or failure cases must NOT return Result<T>

### EXAMPLE (SUCCESS WITH DATA):

```code
public Result<VehicleResponse> GetVehicle(Guid id)
{
    var vehicle = FindVehicle(id);
    return Result<VehicleResponse>.Ok(vehicle);
}
```

### BAD PRACTICE (DO NOT DO THIS):

```code
return new Result<VehicleResponse>
{
    Status = ResultStatus.BadRequest,
    Data = null
};
```

## LISTRESULT<T>

### Use ListResult<T> when:
- Returning a collection
- Paging metadata is NOT required

### Properties:
- Items : list of data
- TotalCount : total number of items

### EXAMPLE:

```code
return new ListResult<VehicleResponse>
{
    Status = ResultStatus.Success,
    Items = vehicles,
    TotalCount = vehicles.Count
};
```

### Typical use cases:
- Dropdown data
- Simple list endpoints
- Master data APIs

## PAGEDRESULT<T>

### Use PagedResult<T> when:
- Returning a collection WITH paging metadata
- UI needs page information

### Properties:
- Items : list of data
- PageInfo : paging metadata

### PageInfo properties:
- Page : current page
- PageSize : items per page
- TotalItems : total data count

### EXAMPLE:

```code
return new PagedResult<VehicleResponse>
{
    Status = ResultStatus.Success,
    Items = vehicles,
    PageInfo = new PageInfo
    {
        Page = request.Paging.Page,
        PageSize = request.Paging.PageSize,
        TotalItems = totalCount
    }
};
```

### Typical use cases:
- Admin panels
- Data tables
- Search results with pagination

## VALIDATIONRESULT
ValidationResult is a specialized result type used ONLY
for input validation errors.

### ValidationResult contains:
- Status : usually BadRequest
- Errors : list of field-level validation errors

### ValidationError properties:
- Field : name of the invalid field
- Message : error description

### EXAMPLE:
```code
return new ValidationResult
{
    Status = ResultStatus.BadRequest,
    Errors = new[]
    {
        new ValidationError("EngineNumber", "Engine number is required"),
        new ValidationError("Lot", "Lot must be greater than zero")
    }
};
```

### IMPORTANT RULES:
- ValidationResult is NOT generic
- ValidationResult replaces Result<T> for invalid input
- ValidationResult should not contain business errors

## END-TO-END FLOW EXAMPLE
Handler / Service example:
```code
public Result Handle(AddVehicleRequest request)
{
    if (string.IsNullOrWhiteSpace(request.EngineNumber))
    {
        return new ValidationResult
        {
            Status = ResultStatus.BadRequest,
            Errors = new[]
            {
                new ValidationError("EngineNumber", "Engine number is required")
            }
    };
}

var vehicle = new VehicleResponse
{
    Id = Guid.NewGuid(),
    EngineNumber = request.EngineNumber
};

return Result<VehicleResponse>.Ok(vehicle);

}
```

## CONTROLLER / TRANSPORT MAPPING
Result models are independent of transport.
```Example HTTP mapping:
if (result.Status == Success)
    return HTTP 200
else if (result.Status == BadRequest)
    return HTTP 400
else if (result.Status == NotFound)
    return HTTP 404
else
    return HTTP 500
```
Controller logic should not depend on Result<T> vs ValidationResult,
only on ResultStatus.

## ANTI-PATTERNS (DO NOT USE)
- Returning Result<T> with null Data for validation errors
- Throwing exceptions for expected validation failures
- Mixing validation messages into generic Message strings
- Using exceptions as flow control

## DESIGN PRINCIPLES
- Result models represent WHAT happened, not HOW
- Use explicit result types instead of implicit assumptions
- Prefer predictable response shapes
- Avoid ambiguity between success and failure responses

## SUMMARY

### Use:
- Result for commands without data
- Result<T> for successful operations with data
- ListResult<T> for collections without paging
- PagedResult<T> for collections with paging
- ValidationResult for structured validation errors

### Avoid:
- Using Result<T> for failure cases
- Using exceptions for normal validation flow

This document exists to ensure result handling in NDB.Abstraction
is consistent, predictable, and safe for long-term usage.