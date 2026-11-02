# Notification System

The application stores entity-operation notifications in the SQL Server `Notification` table. Authenticated users can view unread notifications in the dashboard.

## Behavior

- Student, course, instructor, and department create, update, and delete operations create notifications.
- The browser polls for notifications every five seconds and displays up to five at a time.
- Delivered notifications are marked read by an antiforgery-protected POST request.
- Notification messages are persisted with the application's EF Core database context.

## Authentication

The application uses Windows Negotiate authentication and requires an authenticated user for controller endpoints. It does not currently define role-based authorization; the dashboard is available to any authenticated user.

## Development

Run the app with `dotnet run --project ContosoUniversity.csproj` from the `ContosoUniversity` directory. The default database connection is configured in `appsettings.json`. See `SETUP_TESTING_GUIDE.md` for a manual verification checklist.
