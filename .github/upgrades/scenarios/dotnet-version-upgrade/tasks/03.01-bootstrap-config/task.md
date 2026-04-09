# 03.01-bootstrap-config: Rebuild project bootstrap and configuration for ASP.NET Core

# 03.01-bootstrap-config

## Objective
Convert the project file, startup path, package graph, and configuration surface to ASP.NET Core on .NET 10.

## Scope
- ContosoUniversity.csproj
- Program.cs and app startup files
- Web.config / appsettings.json
- Data/SchoolContextFactory.cs and EF Core startup wiring

## Steps
1. Replace the interim net48 SDK-style project configuration with the final net10.0 package and project setup.
2. Introduce ASP.NET Core hosting, DI registration, EF Core setup, and database initialization.
3. Move configuration from Web.config to appsettings.json and remove obsolete startup artifacts.
4. Build the project and fix all warnings in the touched files.

## Research

- The current project file is already SDK-style but still targets `net48` and carries many legacy package references that will be removed or upgraded in this subtask.
- `Global.asax.cs` currently initializes MVC routing, bundling, and EF Core startup; all of that must move into `Program.cs`.
- `SchoolContextFactory` reads `ConfigurationManager.ConnectionStrings`, so database configuration must move to `IConfiguration`.
- `DbInitializer` already uses `EnsureCreated`, making it straightforward to wire into ASP.NET Core startup.
- The ASP.NET Core MVC template generated with `dotnet new mvc --framework net10.0` provides the baseline hosting pattern (`AddControllersWithViews`, `MapStaticAssets`, `MapControllerRoute`).

## Outcome

- Replaced the interim project file with a minimal ASP.NET Core Web SDK project targeting `net10.0`
- Added `Program.cs` and `appsettings.json` for ASP.NET Core hosting and configuration
- Removed legacy startup/configuration artifacts (`Global.asax*`, `App_Start/*`, `Web.config`, `Views/Web.config`, and the old context factory)
- Temporarily excluded unmigrated controllers and the notification service from compilation so the project can build incrementally during the remaining controller/service subtasks
- Validation result: `dotnet build ContosoUniversity.csproj -warnaserror` succeeded with 0 warnings and 0 errors
