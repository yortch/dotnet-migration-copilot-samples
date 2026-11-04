# Notification System

Administrators receive notifications when entities (students, courses, instructors, departments) are created, updated or deleted.

## Architecture

- **NotificationService** (`Services/NotificationService.cs`): singleton registered in `Program.cs`. Notifications are held in an in-memory `ConcurrentQueue`, which replaces the former MSMQ queue so the app has no Windows-only dependency.
- **NotificationsController**: exposes `GetNotifications` (JSON, PascalCase) and `MarkAsRead`.
- **Layout script**: polls the controller and shows the notification badge/dropdown.

## Notes

- Notifications are non-blocking: failures never break the main operation.
- The queue is per-process and not persisted; it is cleared on restart and not shared across instances. For multi-instance deployments use a shared broker (e.g. Azure Service Bus).
