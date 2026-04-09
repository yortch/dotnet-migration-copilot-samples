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
