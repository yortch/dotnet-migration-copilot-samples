# 03.07-departments-controller: Migrate DepartmentsController and its views

# 03.07-departments-controller

## Objective
Move DepartmentsController and its views to ASP.NET Core MVC.

## Scope
- Controllers/DepartmentsController.cs
- Views/Departments/*
- Related models/view models used by the department workflow

## Steps
1. Update action signatures, model binding, and MVC result handling.
2. Replace any legacy file, routing, or view helper usage.
3. Build the project and fix all warnings in touched files.

## Research

- `DepartmentsController` mainly needs MVC result updates plus drop-down population via `SelectList`.
- The optimistic concurrency flow can stay intact because EF Core still provides `GetDatabaseValues()` for concurrency exceptions.

## Outcome

- Reintroduced `DepartmentsController` into the build and migrated its CRUD flow to ASP.NET Core MVC action/result patterns.
- Preserved the concurrency conflict behavior while updating drop-down population and result helpers for ASP.NET Core.
- Validation result: `dotnet build ContosoUniversity.csproj -warnaserror` succeeded with 0 warnings and 0 errors.
