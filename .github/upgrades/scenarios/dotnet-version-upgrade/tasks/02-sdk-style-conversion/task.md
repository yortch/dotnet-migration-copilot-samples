# 02-sdk-style-conversion: Convert the web project to SDK-style on its current framework

## Objective

Convert `ContosoUniversity.csproj` from the legacy ASP.NET web application format to SDK-style without changing its target framework yet, so the later .NET 10 rewrite can use PackageReference and modern project tooling.

## Scope Inventory

- **Projects affected**: `ContosoUniversity/ContosoUniversity.csproj`
- **Distinct concerns**:
  - Structural project conversion only
  - `packages.config` migration to PackageReference
  - Removing legacy web application imports and redundant assembly references
  - Re-establishing a build path after the current `Microsoft.WebApplication.targets` failure
- **Change signals**:
  - `Project.0001`: project file needs SDK-style conversion
  - `Project.0002`: target framework change is still pending and explicitly out of scope for this task
  - `NuGet.0001`, `NuGet.0002`, `NuGet.0003`, `NuGet.0004`, `NuGet.0005`: package inventory must survive the format conversion so later upgrade work can apply the recommended package actions
- **Skill matches**:
  - `converting-to-sdk-style`
  - `building-projects`

## Ordering and Dependencies

- Topological order from the solution tools contains one project only: `ContosoUniversity/ContosoUniversity.csproj`
- Package inventory is currently split across `packages.config` and explicit `<Reference>` items in `ContosoUniversity.csproj`
- The current build is blocked by the legacy web application targets import, so removing that dependency is the main validation goal for this task

## Files in Scope

- `ContosoUniversity/ContosoUniversity.csproj`
- `ContosoUniversity/packages.config`
- Generated intermediate files under `obj/` may change as a result of the conversion

## Conversion Notes

- Keep the project on `.NET Framework 4.8` during this task
- Do not update package versions yet; preserve the existing dependency set while moving to SDK-style
- After conversion, validate the project directly instead of the full solution to minimize noise

## Conversion Outcome

- `convert_project_to_sdk_style` successfully rewrote `ContosoUniversity.csproj` to SDK-style and moved package definitions into `<PackageReference>` items
- The tool initially left `packages.config` on disk; it has been removed manually so the SDK-style project can become the authoritative package source
- The converted project no longer depends on `Microsoft.WebApplication.targets`, but direct `dotnet build` on `net48` is still blocked by restore-time package issues

## Current Blockers

- `Microsoft.Data.SqlClient` must be upgraded to remove the known vulnerability surfaced as `NU1903`
- The package graph still has downgrades (`NU1605`) against `Microsoft.Bcl.AsyncInterfaces`, `Microsoft.Extensions.Caching.Memory`, `Microsoft.Extensions.Logging.Abstractions`, `System.Diagnostics.DiagnosticSource`, and `System.Memory`
- Because resolving those downgrades meaningfully overlaps the .NET 10 package upgrade work, the task will roll forward into the application rewrite instead of trying to stabilize the interim `net48` project with broad package churn
