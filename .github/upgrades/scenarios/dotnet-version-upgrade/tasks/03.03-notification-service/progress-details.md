# 03.03-notification-service Progress Details

- Replaced the `System.Messaging` implementation in `NotificationService.cs` with a database-backed service that persists notifications through `SchoolContext`.
- Registered the service in `Program.cs` and re-enabled the file in the project build.
- Added APIs for retrieving unread notifications and marking them as read.
- Validated the subtask with `dotnet build ContosoUniversity.csproj -warnaserror` (0 warnings, 0 errors).
