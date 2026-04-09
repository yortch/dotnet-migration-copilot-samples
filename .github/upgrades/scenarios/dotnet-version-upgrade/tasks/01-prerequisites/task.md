# 01-prerequisites: Verify upgrade prerequisites

## Objective

Capture the current toolchain and build baseline for ContosoUniversity before structural project changes begin, and document the legacy assets that drive the .NET 10 rewrite.

## Scope Inventory

- **Projects affected**: `ContosoUniversity/ContosoUniversity.csproj`
- **Distinct concerns**:
  - Build tooling and baseline validation
  - Legacy project-system inventory (`packages.config`, classic web targets)
  - ASP.NET MVC startup and routing entry points
  - Legacy infrastructure to replace (`System.Web.Optimization`, `System.Messaging`, `Web.config`)
- **Change signals**:
  - `Project.0001`: project file must be converted to SDK-style
  - `Project.0002`: target framework must change from `net48` to `net10.0`
  - `Feature.0001`: bundling/minification via `System.Web.Optimization` must be replaced
  - `Feature.0002`: `RouteCollection` registration must move to ASP.NET Core endpoint routing
  - `Feature.0008`: `System.Messaging` usage must be replaced for .NET
  - `Feature.1000`: `Global.asax.cs` initialization must move to `Program.cs`
  - `NuGet.0001`: `Microsoft.AspNet.Web.Optimization` has no supported version
  - `NuGet.0004`: `Microsoft.Data.SqlClient` upgrade required for security
  - `Api.0001` / `Api.0002`: 92 breaking framework API findings across the web app
- **Skill matches**:
  - `building-projects` for build-tool selection and baseline failure handling

## Affected Files

- `ContosoUniversity/ContosoUniversity.csproj` — legacy web application project using classic imports and assembly references
- `ContosoUniversity/packages.config` — package inventory to replace with PackageReference
- `ContosoUniversity/Global.asax` and `ContosoUniversity/Global.asax.cs` — legacy startup path and EF Core initialization
- `ContosoUniversity/App_Start/RouteConfig.cs` — MVC route registration
- `ContosoUniversity/App_Start/BundleConfig.cs` — bundling/minification to remove
- `ContosoUniversity/Web.config` — configuration and binding redirects to migrate
- `ContosoUniversity/Services/NotificationService.cs` — `System.Messaging` queue implementation to replace
- `ContosoUniversity/Controllers/*.cs` — seven MVC controllers that will move to ASP.NET Core MVC

## Assessment Findings

- Project kind: classic ASP.NET Web Application (`Wap`), not SDK-style
- Package issues: 40 total, including 1 incompatible package, 24 recommended upgrades, 11 framework-covered packages, 1 vulnerability, and 3 deprecated packages
- Technologies flagged by the assessment: MSMQ, legacy XML configuration, and `System.Web`
- Estimated rewrite impact: at least 92 lines directly affected across 15 files with incidents

## Baseline Validation

- Installed SDKs include .NET 10 (`10.0.105`, `10.0.201`)
- No test projects were found under the repository
- `msbuild.exe` is not available on PATH in this environment
- Baseline command:

  ```powershell
  dotnet restore .\ContosoUniversity.sln
  dotnet build .\ContosoUniversity.sln -warnaserror
  ```

- Baseline result: restore reported nothing to do, and build failed with `MSB4019` because the legacy project imports `Microsoft.WebApplication.targets`, which is unavailable under the .NET SDK MSBuild path

## Execution Notes

- The upgrade can proceed with `dotnet build` after SDK-style conversion removes the classic web application targets dependency
- Because there are no test projects, validation will rely on restore/build checks for this sample unless a new existing test artifact is discovered during migration
