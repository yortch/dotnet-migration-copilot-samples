# Contoso University

Contoso University is an ASP.NET Core MVC application targeting .NET 10. It uses Entity Framework Core 10 with SQL Server.

## Features

- Student management with search, sorting, and pagination
- Course, instructor, and department management
- Enrollment statistics
- Teaching-material image uploads
- Database-backed notifications
- Windows authentication using Negotiate

## Requirements

- .NET 10 SDK
- SQL Server LocalDB or a compatible SQL Server instance
- Windows for Negotiate authentication and the default LocalDB connection

## Run

From this directory:

```powershell
dotnet restore
dotnet build ContosoUniversity.sln
dotnet run --project ContosoUniversity.csproj
```

The default connection string is in `appsettings.json`. Override it with the `ConnectionStrings__DefaultConnection` environment variable when using another SQL Server instance.

On startup, the application creates the database if it does not exist and seeds the sample data. `EnsureCreated` does not apply schema changes to an existing database; use a reviewed database migration when changing an established schema.

## Notifications and uploads

Notifications are stored in the `Notification` table and fetched by the browser every five seconds. Marking a notification as read updates its database record. Uploaded images are stored under `Uploads/TeachingMaterials` outside the public static-file directory and are served through an authenticated controller endpoint.

Static CSS and JavaScript assets are served from `wwwroot/`.
