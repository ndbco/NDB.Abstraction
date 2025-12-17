# NDB.Abstraction – Validation Guide
This document explains how VALIDATION is represented in NDB.Abstraction,
why it exists, and how it should be used correctly with Result models.

Validation in NDB.Abstraction is about STRUCTURE and COMMUNICATION,
not about how validation rules are implemented.

## PURPOSE OF VALIDATION MODELS

### Validation models are used to:
- Represent input validation errors in a structured way
- Communicate field-level errors to clients (UI, API consumers, SDKs)
- Standardize validation error responses across services
- Separate validation concerns from business logic

### Validation models are NOT used to:
- Perform validation logic
- Replace FluentValidation or other validation libraries
- Enforce business rules
- Replace exceptions for unexpected errors

## CORE VALIDATION TYPES

### NDB.Abstraction provides two validation-related types:
- ValidationResult
- ValidationError

## VALIDATIONERROR

ValidationError represents a single validation issue.

### Properties:
- Field : name of the invalid field
- Message : human-readable error message

### Example:
```code
new ValidationError("EngineNumber", "Engine number is required")
```
Purpose:
- Allows UI to highlight specific fields
- Enables consistent error handling across clients

## VALIDATIONRESULT

ValidationResult is a specialized Result used ONLY for validation failures.

### ValidationResult contains:
- Status : usually BadRequest
- Errors : list of ValidationError objects

ValidationResult INHERITS from Result.

## BASIC VALIDATION EXAMPLE

### Example handler or service method:
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
					new ValidationError(
						"EngineNumber",
						"Engine number is required"
				)
			}
		};
	}
	return Result.Ok("Validation passed");
}
```
Key points:
- ValidationResult is returned INSTEAD of Result<T>
- No exception is thrown
- Validation logic can be simple or complex

## MULTIPLE VALIDATION ERRORS

ValidationResult supports multiple field errors.

### Example:
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
This allows clients to:
- Display multiple errors at once
- Highlight multiple fields

## INTEGRATION WITH VALIDATION LIBRARIES
ValidationResult does NOT replace validation frameworks.
### Example with FluentValidation (conceptual):
```code
var validation = validator.Validate(request);

if (!validation.IsValid)
{
	return new ValidationResult
	{
		Status = ResultStatus.BadRequest,
		Errors = validation.Errors.Select(e =>
				new ValidationError(e.PropertyName, e.ErrorMessage)
		).ToList()
	};
}
```
This shows that:
- Any validation framework can be used
- ValidationResult only defines the OUTPUT format

## WHERE VALIDATION FITS IN THE FLOW

### Typical request flow:
Client sends request
```flow
-> Request DTO
-> Validation logic (manual or library)
-> ValidationResult (if invalid)
-> Business logic
-> Result or Result<T> (if valid)
```
ValidationResult is part of the RESULT layer,
not part of the REQUEST layer.

## CONTROLLER / TRANSPORT HANDLING

Controllers should not treat ValidationResult differently
from other Result types.
### Example mapping:
```code
if (result.Status == BadRequest)
	return HTTP 400 with result
else if (result.Status == Success)
	return HTTP 200 with result
```
The controller does not need to inspect Errors explicitly.

## ANTI-PATTERNS (DO NOT USE)
- Returning Result<T> with null Data for validation errors
- Encoding validation errors inside Message strings
- Throwing exceptions for expected validation failures
- Mixing validation errors with business errors

## DESIGN PRINCIPLES
- ValidationResult represents INVALID INPUT, not system failure
- Validation errors should be predictable and structured
- Validation output should be consistent across endpoints
- Validation logic and validation output are separate concerns

## SUMMARY

### Use ValidationResult when:
- Input data is invalid
- Client needs field-level error information
- Validation failure is expected and recoverable

### Do NOT use ValidationResult when:
- A system error occurs
- An unexpected exception happens
- Business logic fails after validation

This document defines a clear and consistent approach
to handling validation output in NDB.Abstraction.