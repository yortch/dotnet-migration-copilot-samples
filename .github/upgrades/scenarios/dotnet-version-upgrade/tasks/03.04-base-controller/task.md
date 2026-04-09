# 03.04-base-controller: Migrate shared controller infrastructure

# 03.04-base-controller

## Objective
Update the shared controller base and helper behaviors for ASP.NET Core MVC.

## Scope
- Controllers/BaseController.cs
- Shared upload, notification, and helper methods used across derived controllers

## Steps
1. Replace legacy MVC base types and request/file abstractions with ASP.NET Core equivalents.
2. Keep derived controllers compatible with the updated base behavior.
3. Build the project and fix all warnings in touched files.

## Research

- `BaseController` previously instantiated `SchoolContext` and `NotificationService` directly and disposed them manually.
- The new ASP.NET Core project already has both services available through DI, so the base controller should switch to constructor injection.

## Outcome

- Reintroduced `BaseController.cs` into the build and migrated it to `Microsoft.AspNetCore.Mvc.Controller`.
- Replaced manual service creation with constructor injection for `SchoolContext` and `NotificationService`.
- Kept the shared notification helper behavior while routing failures through ASP.NET Core logging.
