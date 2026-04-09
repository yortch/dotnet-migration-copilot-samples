# 03.08-instructors-controller Progress Details

- Added `InstructorsController.cs` back into the project build and migrated it to ASP.NET Core MVC.
- Updated the edit workflow to use `TryUpdateModelAsync` while retaining the assigned-course selection logic and notification hooks.
- Validated the subtask with `dotnet build ContosoUniversity.csproj -warnaserror` (0 warnings, 0 errors).
