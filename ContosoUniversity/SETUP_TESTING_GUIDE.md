# Setup and Testing Guide

## Prerequisites
- .NET 10 SDK
- SQL Server LocalDB

## Run
```
cd ContosoUniversity
dotnet run
```

The database is created and seeded on first start.

## Test the notification system
1. Create, edit or delete a student, course, instructor or department.
2. Open the notification bell in the navigation bar, or browse to `/Notifications/GetNotifications`.
3. Confirm the new notifications are listed; mark them read from the dropdown.

## Troubleshooting
- **LocalDB connection errors**: verify `sqllocaldb info` lists `MSSQLLocalDB` and that the connection string in `appsettings.json` is correct.
- **Notifications missing after restart**: expected, the queue is in-memory.
