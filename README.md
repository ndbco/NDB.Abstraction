# NDB.Abstraction

Framework-agnostic request and result contracts for modern .NET applications.

## Overview

**NDB.Abstraction** provides a set of lightweight, stable DTOs and contracts
designed to be shared across **UI**, **API**, and **client applications**.

This package focuses on **requests and results only** and intentionally contains:
- No database access
- No framework dependencies
- No business logic
- No infrastructure concerns

It is suitable for REST APIs, Blazor, MVC, mobile clients, background workers,
and shared client SDKs.

## When should I use this?

Use **NDB.Abstraction** when you need:

- A shared request model between UI and API
- A consistent result pattern across services
- A clean way to represent:
  - filtering
  - sorting
  - paging
  - validation errors
- A stable DTO contract that rarely breaks

---

## Features

-  Standard request models (filtering, sorting, paging)
-  Result and result-of-T patterns
-  Validation error contracts
-  Generic lookup/value objects
-  Framework-agnostic and dependency-free

---

## Design Principles

-  Contracts only (no behavior)
-  Transport-agnostic (HTTP, gRPC, Blazor, etc.)
-  Stable API surface with minimal breaking changes
-  Intended to be composed with other libraries, not to replace them

---

## Installation

```powershell
dotnet add package NDB.Abstraction 
or 
Install-Package NDB.Abstraction
```

## Usage
```List Request Example
public class SearchVehicleRequest : ListRequest
{
    public string? EngineNumber { get; set; }
}
```

```Result Example
public Result<VehicleResponse> Handle(SearchVehicleRequest request)
{
    // application logic
    return Result.Ok(vehicle);
}
```

