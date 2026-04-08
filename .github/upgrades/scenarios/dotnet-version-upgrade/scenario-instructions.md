# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0

## Source Control
- **Source Branch**: copilot/upgrade-to-net10-cca-dryrun-5
- **Working Branch**: copilot/upgrade-to-net10-cca-dryrun-5
- **Commit Strategy**: Single Commit at End

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Project Structure
- Project Approach: In-place rewrite

### Compatibility
- Unsupported Packages: Resolve Inline (1 incompatible package)
- Unsupported API Handling: Fix Inline
- System.Web Adapters: Direct Migration to ASP.NET Core APIs

### Modernization
- Configuration Migration: Auto-migrate to .NET Core Configuration
- Assembly Binding Redirects: Remove Binding Redirects
- Nullable Reference Types: Leave Disabled

## Strategy
**Selected**: All-at-Once
**Rationale**: Single .NET Framework 4.8 project — no dependency graph to manage.

### Execution Constraints
- Single atomic upgrade — all changes applied together
- SDK-style conversion first, then TFM upgrade and code migration
- Validate full solution build after upgrade
- Fix all build warnings before completing
