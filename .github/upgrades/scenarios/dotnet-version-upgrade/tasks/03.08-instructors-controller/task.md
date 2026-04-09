# 03.08-instructors-controller: Migrate InstructorsController and its views

# 03.08-instructors-controller

## Objective
Move InstructorsController and its views to ASP.NET Core MVC.

## Scope
- Controllers/InstructorsController.cs
- Views/Instructors/*
- Related models/view models used by the instructor workflow

## Steps
1. Update action signatures, model binding, and MVC result handling.
2. Replace any legacy file, routing, or view helper usage, including file upload flows.
3. Build the project and fix all warnings in touched files.

## Research

- `InstructorsController` is the only controller using `TryUpdateModel`, so it needs the ASP.NET Core `TryUpdateModelAsync` flow.
- The instructor/course assignment UI already uses `AssignedCourseData`, which can remain unchanged once `ViewData["Courses"]` is repopulated.

## Outcome

- Reintroduced `InstructorsController` into the build and migrated its list/detail/create/edit/delete actions to ASP.NET Core MVC.
- Replaced the legacy `TryUpdateModel` pattern with `TryUpdateModelAsync` and preserved the assigned-course view model flow.
- Validation result: `dotnet build ContosoUniversity.csproj -warnaserror` succeeded with 0 warnings and 0 errors.
