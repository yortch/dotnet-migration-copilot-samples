# Setup and Testing

## Prerequisites

- .NET 10 SDK
- SQL Server LocalDB, or another SQL Server instance with a configured connection string

## Build and run

From the `ContosoUniversity` directory:

```powershell
dotnet restore
dotnet build
dotnet run
```

The default connection string is in `appsettings.json`. Set the `ConnectionStrings__DefaultConnection` environment variable to use another SQL Server connection.

## Verify the application

1. Open the local URL printed by `dotnet run`.
2. Visit Students, Courses, Instructors, and Departments and verify the seeded data appears.
3. Create, edit, or delete a record and verify that the operation succeeds.
4. Visit Notifications and verify that the operation creates a notification.
5. Upload an image to a course and verify that it appears on the Courses page.

Notifications are stored only in process memory and are lost on restart. The upload directory is local to the application instance.
