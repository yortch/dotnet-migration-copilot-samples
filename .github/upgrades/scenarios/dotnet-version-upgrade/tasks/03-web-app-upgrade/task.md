# 03-web-app-upgrade: Rewrite the application for ASP.NET Core on .NET 10

## Objective

Move the converted ContosoUniversity project from legacy ASP.NET MVC on `net48` to ASP.NET Core MVC on `net10.0`, including the hosting model, package graph, configuration, static assets, notification infrastructure, controllers, and Razor views.

## Scope Inventory

- **Projects affected**: `ContosoUniversity/ContosoUniversity.csproj`
- **Distinct concerns**:
  - ASP.NET Core project bootstrap (`Program.cs`, project file, package graph, appsettings)
  - Legacy configuration migration from `Web.config`
  - Static assets and bundling replacement
  - `System.Messaging` replacement
  - Controller-by-controller MVC migration
  - Shared controller helpers and upload handling
- **Change signals**:
  - `Feature.1000`: startup must move from `Global.asax.cs` to `Program.cs`
  - `Feature.0002`: route registration must move to endpoint routing
  - `Feature.0001`: bundling must be replaced with direct static asset references
  - `Feature.0008`: MSMQ service must be replaced
  - `Api.0001` / `Api.0002`: framework API breaks across controllers, views, startup, and notification service
  - `NuGet.0001` / `NuGet.0002` / `NuGet.0004` / `NuGet.0005`: package removals, upgrades, and vulnerability cleanup are part of the rewrite
- **Skill matches**:
  - `building-projects`
  - Breakdown hints: controller-per-subtask (MUST with >5 controllers), separate System.Messaging replacement (MUST), early config migration, separate static-asset migration

## Files with Assessment Issues

- `ContosoUniversity.csproj`
- `Global.asax.cs`
- `App_Start/RouteConfig.cs`
- `Services/NotificationService.cs`
- `Data/SchoolContextFactory.cs`
- `Controllers/CoursesController.cs`
- `Views/Shared/_Layout.cshtml`
- CRUD views under `Views/Courses`, `Views/Departments`, `Views/Instructors`, and `Views/Students`

## Decomposition Decision

This task is decomposed because mandatory breakdown hints fired for both controller migration (>5 controllers) and `System.Messaging` replacement, and the work spans multiple distinct concerns that benefit from clean validation boundaries.
