# Contoso University

Contoso University is an ASP.NET Core MVC application targeting .NET 10. It uses Entity Framework Core 10 with SQL Server and includes CRUD pages for students, courses, instructors, and departments.

## Requirements

- .NET 10 SDK
- SQL Server LocalDB or another SQL Server instance

## Run the application

From this directory:

```powershell
dotnet restore
dotnet run
```

By default, the application connects to SQL Server LocalDB using the connection string in `appsettings.json`. Override it by setting the `ConnectionStrings__DefaultConnection` environment variable. On startup, the application creates the database if needed and seeds sample data when no students are present.

Teaching material images are stored under `wwwroot/Uploads/TeachingMaterials`. The notification queue is in memory and is cleared when the application restarts; it is intended for local development and does not provide durable or multi-instance messaging.

## Project structure

- `Controllers/`, `Models/`, and `Views/`: MVC application
- `Data/`: Entity Framework Core context and sample-data initializer
- `Services/`: process-local notification queue
- `Content/` and `Scripts/`: static assets served by ASP.NET Core
