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
- JWT-based authentication with stateless, claims-based authorization
- Dockerized API and database via Docker Compose
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

The schema includes both domain data (e.g. `Train`) and an authentication-related `Users` table used for credential storage and JWT-based login.

## 🔐 Authentication & Authorization

The API implements JWT-based authentication, designed to be stateless and secure:

- **Password hashing** using PBKDF2, avoiding storage of plain-text or reversible passwords
- **Credential validation** during login against securely hashed passwords
- **Claims-based identity** — user identity and roles are embedded directly into the JWT claims
- **Stateless authorization** — once a token is issued, no database lookup is required to authorize subsequent requests, since all necessary identity information is carried in the token itself
- **Token generation** signed and issued upon successful authentication
- **Secure configuration** — all sensitive JWT settings (signing key, issuer, audience, expiration) are stored in environment variables rather than committed to source control

This design improves scalability, since authorization checks don't require a database round-trip, and keeps sensitive configuration out of the codebase.

## 🛠️ Technologies Used

| Technology             | Purpose                                  |
| ----------------------- | ------------------------------------------ |
| .NET 8                 | API & application framework              |
| ASP.NET Core            | REST API                                 |
| Entity Framework Core    | ORM & database access                    |
| PostgreSQL              | Relational database                      |
| MediatR                | CQRS & request handling                  |
| FluentValidation         | Input validation                         |
| JWT (JSON Web Tokens)    | Stateless authentication & authorization |
| PBKDF2                 | Password hashing                         |
| Docker / Docker Compose | Containerized local environment           |
| Swagger / OpenAPI       | API documentation                        |
| Clean Architecture      | Project structure                          |

## 🚀 How to Run (Docker — recommended)

The project ships with a `docker-compose.yml` that runs both PostgreSQL and the API as containers.

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (only needed to run EF Core migrations from your host machine)

### 1. Configure environment variables

Create a `.env` file in the repository root (same folder as `docker-compose.yml`):

```env
POSTGRES_DB=TrainRegistry
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_password
```

These values are consumed by the `postgres` service and are also referenced by the `trainregistry.api` service's connection string in `docker-compose.yml`, so they only need to be set in one place.

The API container also needs its JWT settings. Add these to your `.env` file as well (or set them directly under the `trainregistry.api` service's `environment:` block):

```env
Jwt__Key=your_long_random_signing_key
Jwt__Issuer=TrainRegistry
Jwt__Audience=TrainRegistryClients
Jwt__ExpiresInMinutes=60
```

> ⚠️ **Port conflict note:** if you already have a local PostgreSQL installation running on your machine, it will occupy port `5432` on the host and prevent the container's database from being reachable at `localhost:5432`. The compose file maps the container to host port **5433** (`"5433:5432"`) specifically to avoid this. If you change it back to `5432:5432`, make sure no other PostgreSQL service is running locally, or you'll get `28P01: password authentication failed` even with the correct password.

### 2. Start the containers

```bash
docker compose up -d --build
```

This starts two containers:

| Service | Container | Host port | Notes |
| --- | --- | --- | --- |
| `postgres` | `trainregistry-postgres-1` | `5433` → `5432` | Data persisted in a named volume (`postgres_data`) |
| `trainregistry.api` | `trainregistry-trainregistry.api-1` | `5000` → `8080` | Connects to Postgres internally via `Host=postgres` |

### 3. Apply database migrations

Migrations are run from your host machine (outside the container) against the database's **published** port:

```bash
dotnet ef database update --project TrainRegistry.Infrastructure --startup-project TrainRegistry.API
```

This creates the database schema, including the `Users` table used for authentication. It requires a `ConnectionStrings__DefaultConnection` environment variable available to your shell, using `localhost` and the **published** port:

```
Host=localhost;Port=5433;Database=TrainRegistry;Username=postgres;Password=your_password
```

On Windows: **System Properties → Environment Variables → New (User variable)**, then open a *new* terminal/IDE window so the change is picked up.

> Note this is intentionally different from the connection string the API container itself uses (`Host=postgres;Port=5432`, set in `docker-compose.yml`) — the API talks to Postgres over the internal Docker network, while your local migration tooling talks to it through the host's published port.

### 4. Open the app

```
http://localhost:5000/swagger
```

Create a user via the registration endpoint (the database starts with no users), then authenticate via the login endpoint to obtain a JWT. Use the **Authorize** button in Swagger to attach the token (`Bearer <token>`) to subsequent requests.

### Useful commands

```bash
docker ps                                          # check container status
docker logs trainregistry-trainregistry.api-1       # API startup/connection logs
docker exec -it trainregistry-postgres-1 psql -h localhost -U postgres -d TrainRegistry   # connect to the DB inside the container
docker compose down       # stop containers, keep data
docker compose down -v    # stop containers and wipe the database volume
```

## 🚀 How to Run (without Docker)

You can also run everything locally without containers.

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) installed and running locally

### Configuration

Set the following environment variables before running the application:

| Variable | Value |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | `Host=localhost;Port=5432;Database=TrainRegistry;Username=postgres;Password=your_password` |
| `Jwt__Key` | Your secret signing key (e.g. a long random string) |
| `Jwt__Issuer` | The token issuer, e.g. `TrainRegistry` |
| `Jwt__Audience` | The token audience, e.g. `TrainRegistryClients` |
| `Jwt__ExpiresInMinutes` | Token lifetime in minutes, e.g. `60` |

On Windows, go to **System Properties → Environment Variables → New** and enter each name and value above. Then restart your terminal.

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

   This creates the database schema, including the `Users` table used for authentication.

3. Run the application

   ```bash
   dotnet run --project TrainRegistry.API
   ```

4. Open Swagger UI — the port is displayed in the terminal after running the app, e.g. `https://localhost:5001/swagger`
5. Create a user via the registration endpoint, since the database starts with no users.
6. Authenticate via the login endpoint to obtain a JWT, then use the **Authorize** button in Swagger to attach the token (`Bearer <token>`) to subsequent requests.

## 🩺 Troubleshooting

**`28P01: password authentication failed for user "postgres"` when running migrations**

This almost always means the connection is reaching a *different* PostgreSQL instance than you expect — not that the password is wrong. Common causes:

- A local PostgreSQL Windows service is already listening on port `5432`, intercepting connections meant for the Docker container. Check with:
  ```powershell
  Get-Process -Id (Get-NetTCPConnection -LocalPort 5432).OwningProcess
  ```
  If this shows a `postgres` process, stop it or use the container's remapped port (`5433`) instead.
- The database volume was initialized before `POSTGRES_PASSWORD` was set/changed — Postgres only applies that variable on first init of an empty volume. Fix with `docker compose down -v && docker compose up -d` (⚠️ destroys existing data).
- The `ConnectionStrings__DefaultConnection` environment variable was edited but the terminal/IDE was already open — Windows env var changes only apply to *new* processes. Close and reopen your terminal/IDE.
