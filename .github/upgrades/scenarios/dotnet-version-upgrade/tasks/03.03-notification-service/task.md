# 03.03-notification-service: Replace the System.Messaging notification implementation

# 03.03-notification-service

## Objective
Remove System.Messaging usage and provide a .NET 10-compatible notification service implementation.

## Scope
- Services/NotificationService.cs
- Models/Notification.cs and any related helpers
- Configuration values related to notification storage/queueing

## Steps
1. Replace the MSMQ-based implementation with a supported in-process or application-backed approach.
2. Update serialization and disposal behavior for the new implementation.
3. Adjust any consumers to use the new service shape if needed.
4. Build the project and fix all warnings in touched files.

## Research

- The current service is isolated behind `NotificationService.cs` and only depends on the `Notification` model plus `EntityOperation`.
- The project already has a `Notifications` DbSet on `SchoolContext`, so database-backed persistence is available without introducing a new external queue technology.
- `BaseController` will need this service later, but the bootstrap subtask currently excludes that controller from compilation, so the service can change shape safely now.

## Outcome

- Replaced the MSMQ-based notification service with a database-backed implementation built on `SchoolContext`.
- Registered `NotificationService` with ASP.NET Core DI and removed the old compile exclusion so the service is part of the project again.
- Exposed explicit methods for sending notifications, fetching unread notifications, and marking notifications as read.
- Validation result: `dotnet build ContosoUniversity.csproj -warnaserror` succeeded with 0 warnings and 0 errors.
