# 03.10-students-controller: Migrate StudentsController and its views

# 03.10-students-controller

## Objective
Move StudentsController and its views to ASP.NET Core MVC.

## Scope
- Controllers/StudentsController.cs
- Views/Students/*
- Related models/view models used by the student workflow

## Steps
1. Update action signatures, model binding, and MVC result handling.
2. Replace any legacy file, routing, or view helper usage.
3. Build the project and fix all warnings in touched files.

## Research

- `StudentsController` depends on `PaginatedList<T>` and a search/sort query pipeline, but its migration is mostly mechanical once the MVC result helpers are updated.
- The existing validation logic around `EnrollmentDate` can remain in the controller with minimal changes.

## Outcome

- Reintroduced `StudentsController` into the build and migrated its search/sort/pagination plus CRUD actions to ASP.NET Core MVC.
- Preserved the legacy enrollment-date validation and notification hooks while updating the MVC action/result helpers.
- Validation result: `dotnet build ContosoUniversity.csproj -warnaserror` succeeded with 0 warnings and 0 errors.
