# 03.01-bootstrap-config Progress Details

- Replaced the converted interim project file with a minimal ASP.NET Core Web SDK project targeting `net10.0`.
- Added `Program.cs` and `appsettings.json` to establish ASP.NET Core hosting, EF Core registration, and database initialization.
- Removed obsolete startup and configuration artifacts from the ASP.NET MVC application (`Global.asax*`, `App_Start/*`, `Web.config`, `Views/Web.config`, and `SchoolContextFactory.cs`).
- Excluded the unmigrated controllers and notification service from compilation so the remaining subtasks can migrate them incrementally without breaking the project.
- Validated the subtask with `dotnet build ContosoUniversity.csproj -warnaserror` (0 warnings, 0 errors).
