# 03.05-home-controller Progress Details

- Added `HomeController.cs` back into the project build and migrated its actions to ASP.NET Core MVC.
- Updated the error handling flow to return `ErrorViewModel` and converted the shared error view away from `HandleErrorInfo`.
- Preserved the legacy route name for the unauthorized endpoint with `ActionName(\"Unauthorized\")`.
- Validated the subtask with `dotnet build ContosoUniversity.csproj -warnaserror` (0 warnings, 0 errors).
