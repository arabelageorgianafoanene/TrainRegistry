# TrainRegistry

TrainRegistry is a modular .NET 8 microservice responsible for managing train registration and metadata within a distributed Train Management System.
It exposes a clean REST API, follows Clean Architecture, and integrates modern .NET patterns such as CQRS with MediatR, FluentValidation, and Entity Framework Core with PostgreSQL.

## 🚆 Features

- RESTful Web API built with ASP.NET Core
- Domain-Driven Design structure (Domain, Application, Infrastructure, API)
- CQRS-style request/response handling using MediatR
- FluentValidation for validating commands and queries
- Entity Framework Core for data access
- PostgreSQL as the database provider
- Swagger / OpenAPI for API documentation and testing
- Clear separation of concerns and maintainable architecture

## 🏗️ Architecture Overview

The solution follows Clean Architecture and is split into four projects:

### 1. TrainRegistry.API (ASP.NET Core Web API)
- Exposes REST endpoints
- Handles HTTP requests and responses
- Integrates Swagger/OpenAPI
- Uses MediatR to dispatch commands and queries
- Maps DTOs to domain models

### 2. TrainRegistry.Domain (Class Library)
- Contains core domain entities (e.g., Train)
- Contains domain logic and invariants
- No external dependencies

### 3. TrainRegistry.Application (Class Library)
- Contains CQRS handlers (commands & queries)
- Business logic and use cases
- FluentValidation validators
- Interfaces for repositories
- Mediator pipeline behaviors

### 4. TrainRegistry.Infrastructure (Class Library)
- Implements repository interfaces
- Entity Framework Core DbContext
- PostgreSQL configuration
- Database migrations

## 🗄️ Database

The service uses PostgreSQL as its database engine.

Entity Framework Core is used for:
- Migrations
- Database schema creation
- Querying and persisting domain entities

## 🛠️ Technologies Used

| Technology | Purpose |
|---|---|
| .NET 8 | API & application framework |
| ASP.NET Core | REST API |
| Entity Framework Core | ORM & database access |
| PostgreSQL | Relational database |
| MediatR | CQRS & request handling |
| FluentValidation | Input validation |
| Swagger / OpenAPI | API documentation |
| Clean Architecture | Project structure |

## 🚀 How to Run

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)

### Configuration

Set the following environment variable before running the application:

| Variable | Value |
|---|---|
| `ConnectionStrings__DefaultConnection` | `Host=localhost;Port=5432;Database=TrainRegistry;Username=postgres;Password=your_password` |

On Windows, go to **System Properties → Environment Variables → New** and enter the name and value above. Then restart your terminal.

### Steps

1. Clone the repository
   ```bash
   git clone https://github.com/arabelageorgianafoanene/TrainRegistry.git
   cd TrainRegistry
   ```

2. Apply database migrations
   ```bash
   dotnet ef database update --project TrainRegistry.Infrastructure --startup-project TrainRegistry.API
   ```

3. Run the application
   ```bash
   dotnet run --project TrainRegistry.API
   ```

4. Open Swagger UI — the port is displayed in the terminal after running the app, e.g. `https://localhost:5001/swagger`
