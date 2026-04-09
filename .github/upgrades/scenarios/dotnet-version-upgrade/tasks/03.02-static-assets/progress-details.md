# 03.02-static-assets Progress Details

- Updated `Program.cs` to serve the existing `Content`, `Scripts`, and `Uploads` directories as static file roots in the ASP.NET Core app.
- Replaced the bundle helper usage in `_Layout.cshtml` and the jQuery validation sections in the CRUD views with direct `<link>` and `<script>` tags.
- Added `Views/_ViewImports.cshtml` for ASP.NET Core Razor imports.
- Validated the subtask with `dotnet build ContosoUniversity.csproj -warnaserror` (0 warnings, 0 errors).
