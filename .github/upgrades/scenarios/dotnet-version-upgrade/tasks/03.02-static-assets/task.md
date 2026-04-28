# 03.02-static-assets: Migrate static assets and layout bundling

# 03.02-static-assets

## Objective
Replace System.Web.Optimization bundling with ASP.NET Core static files and layout references.

## Scope
- Content/ and Scripts/ assets
- wwwroot layout as needed
- Views/Shared/_Layout.cshtml and shared view imports
- CRUD views currently using @Scripts.Render or @Styles.Render

## Steps
1. Establish the ASP.NET Core static asset structure.
2. Replace bundle references in shared and CRUD views with direct asset references.
3. Ensure client-side validation and upload assets still resolve correctly.
4. Build the project and fix all warnings in touched files.

## Research

- `_Layout.cshtml` still used `@Styles.Render` and `@Scripts.Render` for the legacy bundles.
- The affected CRUD views only rely on the `~/bundles/jqueryval` bundle and can move to direct script tags.
- Existing static files already live under `Content/`, `Scripts/`, and `Uploads/`, so additional static-file mappings are enough for the interim migration.

## Outcome

- Added static file mappings in `Program.cs` for the existing `Content`, `Scripts`, and `Uploads` folders.
- Replaced bundle helper usage in the shared layout and all affected CRUD views with direct asset references.
- Added `_ViewImports.cshtml` so the ASP.NET Core view system has the expected namespace/tag-helper imports.
- Validation result: `dotnet build ContosoUniversity.csproj -warnaserror` succeeded with 0 warnings and 0 errors.
