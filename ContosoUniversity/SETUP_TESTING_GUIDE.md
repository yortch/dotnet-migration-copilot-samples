# Setup and Testing Guide

## Requirements

- .NET 10 SDK
- SQL Server LocalDB or a compatible SQL Server instance
- Windows authentication support for the configured Negotiate scheme

## Build and run

From the `ContosoUniversity` directory:

```powershell
dotnet restore
dotnet build ContosoUniversity.sln
dotnet run --project ContosoUniversity.csproj
```

The default database is `ContosoUniversityNoAuthEFCore` on LocalDB. Set `ConnectionStrings__DefaultConnection` to override it.

## Verify the application

1. Browse to the HTTPS URL printed by `dotnet run` and authenticate with Windows credentials.
2. Visit Students, Courses, Instructors, Departments, and Notifications.
3. Exercise create, edit, details, and delete flows; verify validation and student search/pagination.
4. Upload an image for a course and verify that it appears under `wwwroot/Uploads/TeachingMaterials`.
5. Perform an entity operation and confirm that a notification appears. Verify the associated `Notification` row is marked read after delivery.

There are no automated test projects in this solution. `dotnet build ContosoUniversity.sln` validates compilation.
