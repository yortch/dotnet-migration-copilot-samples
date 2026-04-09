# 03.09-notifications-controller Progress Details

- Added `NotificationsController.cs` back into the project build and migrated it to ASP.NET Core MVC.
- Updated the JSON endpoints to use the database-backed `NotificationService` methods for unread notifications and read-state updates.
- Validated the subtask with `dotnet build ContosoUniversity.csproj -warnaserror` (0 warnings, 0 errors).
