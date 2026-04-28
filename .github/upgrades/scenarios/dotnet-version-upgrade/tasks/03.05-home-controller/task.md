# 03.05-home-controller: Migrate HomeController and its views

# 03.05-home-controller

## Objective
Move HomeController and related views to ASP.NET Core MVC.

## Scope
- Controllers/HomeController.cs
- Views/Home/*
- Any shared models or helpers used only by HomeController

## Steps
1. Update the controller base type, action signatures, and result helpers.
2. Fix any view/runtime assumptions that depend on System.Web.
3. Build the project and fix all warnings in touched files.

## Research

- `HomeController` depends only on `BaseController`, `SchoolContext`, and `EnrollmentDateGroup`, making it a good first controller to reintroduce.
- `Views/Shared/Error.cshtml` still depends on `System.Web.Mvc.HandleErrorInfo` and `HttpContext.Current`, so it must move to the existing `ErrorViewModel`.

## Outcome

- Reintroduced `HomeController.cs` into the build and migrated it to ASP.NET Core MVC action patterns.
- Updated the shared error view to use `ErrorViewModel` and the ASP.NET Core request identifier instead of `HandleErrorInfo`.
- Validation result: `dotnet build ContosoUniversity.csproj -warnaserror` succeeded with 0 warnings and 0 errors.
