# NDB.Abstraction – Requests Guide

This document explains how REQUEST models in NDB.Abstraction should be used
in a clear, consistent, and non-ambiguous way, including practical code examples.

Request models in NDB.Abstraction act as COMMUNICATION CONTRACTS between
clients and backend services.

## PURPOSE OF REQUEST MODELS
Request models are used to:
- Represent client intent (UI, API consumers, SDKs)
- Standardize how clients request data
- Avoid coupling to databases, ORMs, or EF Core
- Be reusable across UI, API, and client SDKs

Request models are NOT used to:
- Write database queries
- Implement business rules
- Perform validation logic
- Contain EF Core or SQL logic

## CORE REQUEST TYPE
ListRequest is a generic request contract for list-based operations.

ListRequest supports:
- Filtering
- Sorting
- Paging

Properties in ListRequest:
- Filters : list of filter rules
- Sorts : list of sorting rules
- Paging : paging information

## FILTERREQUEST

FilterRequest represents a single filtering rule sent by the client.

Properties:
- Field : logical field name (not necessarily a database column)
- Value : value to be matched
- Operator : comparison operator

Supported operators:
- Equals
- Contains
- StartsWith
- EndsWith
- GreaterThan
- LessThan

Example FilterRequest (conceptual):
Field = "EngineNumber"
Value = "ABC"
Operator = Contains


## SORTREQUEST

SortRequest defines how the result should be ordered.

Properties:
- Field : logical field name
- Direction : Asc or Desc

## PAGINGREQUEST

PagingRequest defines how result data should be paged.

Properties:
- Page : page number (default 1)
- PageSize : number of items per page (default 20)

## OFFICIAL USAGE PATTERNS

There are TWO OFFICIAL request usage patterns.
Choose ONE pattern per endpoint.
DO NOT mix these patterns without explicit rules.

### PATTERN A – GENERIC FILTERING (ADMIN / ENTERPRISE)

Use this pattern when:
- Filter fields are dynamic
- Used by admin panels or data tables
- Flexibility is required

```REQUEST DTO (C#):
using NDB.Abstraction.Requests;

public class SearchVehicleRequest : ListRequest
{
	// No strongly-typed filter properties
}
```

```EXAMPLE JSON REQUEST:
{
	"filters": [
			{ "field": "EngineNumber", "value": "ABC", "operator": "Contains" },
			{ "field": "Status", "value": "ORDER", "operator": "Equals" }
		],
	"sorts": [
		{ "field": "CreatedDate", "direction": "Desc" }
	],
	"paging": {
		"page": 1,
		"pageSize": 20
	}
}
```

```BACKEND USAGE EXAMPLE (PSEUDO-CODE):
foreach (filter in request.Filters)
{
	//apply filter.Field, filter.Operator, filter.Value to query
}
//apply sorting from request.Sorts
//apply paging from request.Paging
```

#### ADVANTAGES:
- Highly flexible
- No DTO changes when new fields are added
- Ideal for enterprise systems

#### DISADVANTAGES:
- Not strongly typed
- Less explicit in Swagger documentation

### PATTERN B – STRONGLY TYPED REQUEST (SIMPLE / PUBLIC API)

Use this pattern when:
- Search criteria are fixed
- API is public or simple
- Clarity is more important than flexibility

```REQUEST DTO (C#):
public class SearchVehicleRequest :PagingRequest
{
	public string? EngineNumber { get; set; }
}
```

```EXAMPLE JSON REQUEST:
{
	"engineNumber": "ABC",
	"page": 1,
	"pageSize": 20
}
```

```BACKEND USAGE EXAMPLE (C#):
var query = vehicles;

if (!string.IsNullOrEmpty(request.EngineNumber))
{
	query = query.Where(x => x.EngineNumber.Contains(request.EngineNumber));
}

var result = query
.Skip((request.Page - 1) * request.PageSize)
.Take(request.PageSize)
.ToList();
```

#### ADVANTAGES:
- Clear and explicit
- Type-safe
- Easy to document

#### DISADVANTAGES:
- Less flexible
- DTO changes required when fields change

## ANTI-PATTERN (DO NOT USE)

DO NOT create request models that:
Inherit from ListRequest AND Declare strongly-typed filter properties

```Example of BAD DESIGN:

public class SearchVehicleRequest : ListRequest
{
	public string? EngineNumber { get; set; }
}
```

Why this is bad:
- Redundant
- Ambiguous
- Developers do not know which filter takes precedence
- API documentation becomes unclear

If this pattern is ever used, explicit rules MUST be documented, such as:
Filters override strongly-typed properties
OR
Strongly-typed properties act only as shortcuts when Filters are empty
Without explicit rules, this pattern MUST be avoided.

## DESIGN PRINCIPLES
- Requests describe WHAT the client wants
- Requests do NOT describe HOW the backend works
- One endpoint equals one request pattern
- Prefer clarity over flexibility when in doubt

## SUMMARY

### Use:
- Generic Filtering for admin and enterprise UIs
- Strongly Typed Requests for simple and public APIs

### Avoid:
- Mixing strongly-typed properties with Filters without clear rules

This document exists to ensure request models in NDB.Abstraction
are used consistently, predictably, and safely over time.