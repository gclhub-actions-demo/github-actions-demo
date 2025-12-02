# .NET Core Web API

An ASP.NET Core Web API demonstrating CI/CD patterns with .NET 8.0.

## Features

- .NET 8.0
- Minimal API pattern
- Swagger/OpenAPI documentation
- xUnit tests with WebApplicationFactory
- Health check endpoint
- Task management REST API

## Prerequisites

- .NET 8.0 SDK or higher

## Building

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Build in Release mode
dotnet build -c Release
```

## Running

```bash
# Run the application
dotnet run

# Run in watch mode (auto-reload)
dotnet watch run
```

The API will be available at `http://localhost:5000` (or `https://localhost:5001`).

## Testing

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## API Endpoints

- `GET /health` - Health check endpoint
- `GET /api/tasks` - Get all tasks
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create a new task
- `PUT /api/tasks/{id}` - Update a task
- `DELETE /api/tasks/{id}` - Delete a task

## Swagger Documentation

When running in development mode, access the Swagger UI at:
- `http://localhost:5000/swagger`

## Example Requests

### Get all tasks
```bash
curl http://localhost:5000/api/tasks
```

### Get a specific task
```bash
curl http://localhost:5000/api/tasks/1
```

### Create a task
```bash
curl -X POST http://localhost:5000/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"New Task","description":"Task description","isCompleted":false}'
```

### Update a task
```bash
curl -X PUT http://localhost:5000/api/tasks/1 \
  -H "Content-Type: application/json" \
  -d '{"title":"Updated Task","isCompleted":true}'
```

### Delete a task
```bash
curl -X DELETE http://localhost:5000/api/tasks/1
```

## Publishing

```bash
# Publish for Linux
dotnet publish -c Release -r linux-x64 --self-contained

# Publish for Windows
dotnet publish -c Release -r win-x64 --self-contained

# Publish framework-dependent
dotnet publish -c Release
```

## Configuration

Application configuration is in `appsettings.json` and `appsettings.Development.json`.
