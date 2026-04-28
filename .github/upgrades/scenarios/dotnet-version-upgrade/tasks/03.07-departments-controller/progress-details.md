# 03.07-departments-controller Progress Details

- Added `DepartmentsController.cs` back into the project build and migrated it to ASP.NET Core MVC.
- Updated the instructor drop-down handling to use ASP.NET Core `SelectList` plumbing and retained the EF Core concurrency resolution flow.
- Validated the subtask with `dotnet build ContosoUniversity.csproj -warnaserror` (0 warnings, 0 errors).
