# NDB.Abstraction – Markers Guide
This document explains the purpose and usage of MARKER INTERFACES
provided by NDB.Abstraction, including practical examples.

Marker interfaces are intentionally minimal and optional.
They exist to support conventions, tooling, and future extensibility.

## WHAT ARE MARKER INTERFACES?

A marker interface is an interface with NO methods or properties.

### Example:
```code
public interface IRequestDto { }
```

It does not define behavior.
It only marks or labels a class.

## WHY MARKERS EXIST IN NDB.Abstraction

### Markers in NDB.Abstraction are used to:
- Explicitly identify request and response DTOs
- Support conventions across large codebases
- Enable reflection-based tooling
- Prepare for future analyzers or code generation
- Improve clarity and intent in architecture

### Markers are NOT used to:
- Add runtime behavior
- Perform validation
- Implement business logic
- Replace inheritance or composition

## AVAILABLE MARKERS

### NDB.Abstraction provides the following marker interfaces:
- IRequestDto
- IResponseDto

Both interfaces are empty by design.

### IREQUESTDTO
IRequestDto marks a class as a REQUEST object.

### Example:
```code
using NDB.Abstraction.Markers;

public class SearchVehicleRequest : IRequestDto
{
	public string? EngineNumber { get; set; }
	public int Page { get; set; }
	public int PageSize { get; set; }
}
```
Purpose:
- Indicates this class represents client input
- Helps distinguish request DTOs from other models

### IRESPONSEDTo

IResponseDto marks a class as a RESPONSE object.

### Example:
```code
using NDB.Abstraction.Markers;

public class VehicleResponse : IResponseDto
{
	public Guid Id { get; set; }
	public string EngineNumber { get; set; }
}
```
Purpose:
- Indicates this class is safe to expose to clients
- Separates response DTOs from domain or entity models

## PRACTICAL USE CASES

Markers are most useful in LARGE or MODULAR systems.
### Example 1: Reflection-based discovery
```code
var requestTypes = assembly.GetTypes()
					.Where(t => typeof(IRequestDto).IsAssignableFrom(t));
```

### This can be used for:
- Automatic documentation
- Validation pipelines
- Logging or tracing
- Code generation

### Example 2: Convention enforcement
You may enforce rules such as:
- Only IRequestDto classes are accepted by handlers
- Only IResponseDto classes are returned to controllers

This helps maintain architectural boundaries.

### Example 3: Future tooling

### Markers allow future extensions such as:
- Roslyn analyzers
- Automatic OpenAPI generation
- Client SDK generation
- Mapping or validation conventions

Markers make these features possible WITHOUT changing existing code.

## DO YOU HAVE TO USE MARKERS?
NO. Markers are OPTIONAL.

### You can safely ignore them if:
- Your project is small
- You do not use reflection-based tooling
- You prefer explicit documentation only

Markers exist to SCALE architecture, not to complicate it.

## ANTI-PATTERNS (DO NOT DO THIS)
- Do not add logic to marker interfaces
- Do not rely on markers for runtime behavior
- Do not assume markers enforce rules by themselves
- Do not use markers as a replacement for validation

## DESIGN PRINCIPLES
- Markers communicate INTENT, not behavior
- They are cheap to add and safe to ignore
- They support consistency in large systems
- They enable future extensibility without breaking changes

## SUMMARY
- Marker interfaces are labels, not logic
- IRequestDto marks request objects
- IResponseDto marks response objects
- Markers are optional but future-proof
- They are most useful in large or long-lived projects

This document explains how and why marker interfaces
exist in NDB.Abstraction and how they can be used safely.