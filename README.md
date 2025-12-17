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

Typical use cases:

- REST APIs
- Blazor / MVC applications
- Background workers
- Client SDKs
- Admin panels and dashboards

---

## When should I NOT use this?

Do **not** use this library for:

- Database access
- ORM helpers
- Business logic
- Validation frameworks
- Mapping logic

Those concerns belong in **separate libraries**.

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
---

## Usage Example
- [Requests](src/Requests/README.md)
- [Results](src/Results/README.md)
- [Markers](src/Markers/README.md)
- [Common](src/Common/README.md)
- [Validation](src/Validation/README.md)