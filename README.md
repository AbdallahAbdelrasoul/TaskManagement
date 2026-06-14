# Task Management API

A RESTful backend API for managing projects and tasks, built as a technical assessment submission.

## Tech Stack

- **.NET 9** — ASP.NET Core Web API
- **Entity Framework Core 9** — SQL Server
- **JWT Bearer** — stateless authentication
- **FluentValidation** — request validation
- **BCrypt.Net** — password hashing
- **Swagger / OpenAPI** — interactive API documentation

---

## Architecture

Clean Architecture with 5 layers:

```
TaskManagement.API              ← Entry point: controllers, middleware, DI wiring
    ↓
TaskManagement.Application      ← Business logic: services, DTOs, validators
    ↓
TaskManagement.Domain           ← Core: entities, interfaces, exceptions, shared models
    ↓
TaskManagement.DataAccess       ← Persistence: EF Core DbContext, UnitOfWork
TaskManagement.Infrastructure   ← Cross-cutting: repositories, JWT token service, password hashing
```

**Key patterns used:**
- Repository pattern + Unit of Work
- Dependency Injection throughout
- Global exception handler middleware (maps domain exceptions to HTTP status codes)
- Generic API response wrapper (`ApiResponse<T>`)
- FluentValidation on all request DTOs
- Role-based authorization (Admin / User)

---

## Setup Instructions

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)

### 1. Configure the database connection

Edit `TaskManagement.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Replace with your SQL Server connection string if needed.

### 2. Configure JWT settings

In `appsettings.json`, update the secret key (must be at least 32 characters):

```json
"JwtSettings": {
  "SecretKey": "YOUR_STRONG_SECRET_KEY_AT_LEAST_32_CHARS",
  "Issuer": "TaskManagement.API",
  "Audience": "TaskManagement.Clients",
  "ExpiryMinutes": 60
}
```

### 3. Apply the database migration

```bash
dotnet ef database update --project TaskManagement.DataAccess --startup-project TaskManagement.API
```

### 4. Run the API

```bash
dotnet run --project TaskManagement.API
```

The API will be available at:
- HTTP: `http://localhost:5139`
- HTTPS: `https://localhost:7140`
- Swagger UI: `https://localhost:7140/swagger`

---

## API Endpoints

### Authentication

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/register` | None | Register a new user |
| POST | `/api/auth/login` | None | Login and receive JWT token |

### Projects

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/projects` | Required | Get all projects (paginated) |
| GET | `/api/projects/{id}` | Required | Get project by ID |
| POST | `/api/projects` | Required | Create a new project |
| PUT | `/api/projects/{id}` | Required | Update a project |
| DELETE | `/api/projects/{id}` | Required | Delete a project |
| GET | `/api/projects/{id}/tasks` | Required | Get tasks for a project |

### Tasks

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/tasks` | Required | Get all tasks (filter by `projectId`, `status`) |
| GET | `/api/tasks/{id}` | Required | Get task by ID |
| POST | `/api/tasks` | Required | Create a new task |
| PUT | `/api/tasks/{id}` | Required | Update a task |
| DELETE | `/api/tasks/{id}` | Required | Delete a task |

### Users

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/users` | Admin only | List all users (paginated) |
| GET | `/api/users/{id}` | Required | Get user by ID |
| PUT | `/api/users/{id}` | Required | Update user |
| DELETE | `/api/users/{id}` | Admin only | Delete user |

**Pagination** — all list endpoints accept `?pageNumber=1&pageSize=10` query params.

**Task status values:** `Todo`, `InProgress`, `InReview`, `Done`, `Cancelled`

**Task priority values:** `Low`, `Medium`, `High`, `Critical`

---

## Using Swagger with JWT

1. Run the API and open `https://localhost:7140/swagger`
2. Call `POST /api/auth/register` to create an account
3. Call `POST /api/auth/login` to get your JWT token
4. Click the **Authorize** button (top right in Swagger UI)
5. Enter: `Bearer <your-token>`
6. All subsequent requests will include the token

---

## Request / Response Examples

### Register
```json
POST /api/auth/register
{
  "username": "john",
  "email": "john@example.com",
  "password": "Password1",
  "confirmPassword": "Password1"
}
```

### Create Project
```json
POST /api/projects
{
  "name": "My Project",
  "description": "Project description"
}
```

### Create Task
```json
POST /api/tasks
{
  "title": "Implement login",
  "description": "Add JWT login endpoint",
  "priority": "High",
  "dueDate": "2026-07-01T00:00:00Z",
  "projectId": 1
}
```

### Update Task Status
```json
PUT /api/tasks/1
{
  "title": "Implement login",
  "description": "Add JWT login endpoint",
  "status": "Done",
  "priority": "High",
  "dueDate": "2026-07-01T00:00:00Z"
}
```

### Response Envelope
All responses use a consistent wrapper:
```json
{
  "success": true,
  "message": "Task created successfully.",
  "data": { ... },
  "errors": null
}
```
