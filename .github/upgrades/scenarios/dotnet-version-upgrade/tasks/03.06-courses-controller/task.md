# 03.06-courses-controller: Migrate CoursesController and its views

# 03.06-courses-controller

## Objective
Move CoursesController and the course CRUD views to ASP.NET Core MVC.

## Scope
- Controllers/CoursesController.cs
- Views/Courses/*
- Related models/view models used by the course workflow

## Steps
1. Update action signatures, model binding, and MVC result handling.
2. Replace any legacy file, routing, or view helper usage.
3. Build the project and fix all warnings in touched files.

## Research

- `CoursesController` is the only controller using `HttpPostedFileBase` and `Server.MapPath`, so it requires the upload-path conversion to ASP.NET Core file handling.
- The existing course views already carry the upload form field and can keep that UI once the controller moves to `IFormFile`.

## Outcome

- Reintroduced `CoursesController` into the build and migrated its CRUD actions to ASP.NET Core MVC action/result patterns.
- Replaced legacy upload handling with `IFormFile`, `IWebHostEnvironment`, and content-root file paths under `Uploads/TeachingMaterials`.
- Validation result: `dotnet build ContosoUniversity.csproj -warnaserror` succeeded with 0 warnings and 0 errors.
