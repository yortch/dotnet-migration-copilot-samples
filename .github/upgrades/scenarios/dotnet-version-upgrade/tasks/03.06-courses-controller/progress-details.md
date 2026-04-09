# 03.06-courses-controller Progress Details

- Added `CoursesController.cs` back into the project build and migrated its actions to ASP.NET Core MVC.
- Replaced the legacy file-upload flow with `IFormFile`, content-root file storage, and helper methods for saving/removing teaching material images.
- Kept the existing course views usable by preserving the posted model shape and department drop-down population.
- Validated the subtask with `dotnet build ContosoUniversity.csproj -warnaserror` (0 warnings, 0 errors).
