# .NET Version Upgrade

## Strategy
**Selected**: All-at-Once
**Rationale**: Single .NET Framework MVC project with no project-to-project dependencies; an in-place rewrite keeps the sample concise while avoiding phased coordination overhead.

### Execution Constraints
- Convert the legacy web project to SDK-style before changing its target framework.
- Keep package replacement, API migration, and project-system changes in the same upgrade pass so the solution reaches a single compilable ASP.NET Core state.
- Remove legacy web-specific infrastructure such as Global.asax, RouteConfig, BundleConfig, and binding redirects as part of the rewrite.
- Treat build warnings as errors for modified code and do not finish a task until the upgraded project builds warning-free.
- Final validation must include restore, build, and any available automated checks on the upgraded solution.

## Preferences
- **Flow Mode**: Automatic
- **Commit Strategy**: After Each Task
- **Pace**: Standard
- **Target Framework**: net10.0

## Source Control
- **Source Branch**: copilot/upgrade-to-net10-cca-live
- **Working Branch**: upgrade-to-net10-cca-live

## Build Tool Decisions
- **ContosoUniversity.csproj**: Visual Studio MSBuild for the legacy project format; `dotnet build` currently fails with MSB4019 on `Microsoft.WebApplication.targets`, so use `dotnet build` only after SDK-style conversion.

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Project Structure
- Project Approach: Web Projects = In-place rewrite

### Compatibility
- Unsupported Packages: Resolve Inline (2 incompatible packages)
- Unsupported API Handling: Fix Inline
- System.Web Adapters: Direct Migration to ASP.NET Core APIs

### Modernization
- Configuration Migration: Auto-migrate to .NET Core Configuration
- Assembly Binding Redirects: Remove Binding Redirects
- Nullable Reference Types: Leave Disabled

## Decisions
- Use the user-requested .NET 10 target and work under the requested upgrade branch name without pausing for interactive option confirmation.

## Custom Instructions
