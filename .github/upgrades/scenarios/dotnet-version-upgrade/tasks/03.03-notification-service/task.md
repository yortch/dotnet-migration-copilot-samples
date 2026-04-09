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
