# Scenario Instructions — .NET Version Upgrade

## Scenario
**Target**: Upgrade ContosoUniversity to .NET 10 (net10.0)
**Solution**: D:\a\dotnet-migration-copilot-samples\dotnet-migration-copilot-samples\ContosoUniversity\ContosoUniversity.sln

## Source Control
**Working Branch**: copilot/upgrade-net10-cca-jul-29
**Commit Strategy**: After Each Task

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Project Structure
- Project Approach: Side-by-side (Web Projects), In-place (Class Libraries)

### Compatibility
- Unsupported Packages: Resolve Inline (2 incompatible packages)
- Unsupported API Handling: Fix Inline
- Windows Native APIs: Windows Compatibility Pack
- System.Web Adapters: Use System.Web Adapters
  Skill: aspnet-system-web-adapters

### Modernization
- Assembly Binding Redirects: Document and Review Before Removing
- Nullable Reference Types: Leave Disabled

## Strategy
**Selected**: All-at-Once
**Rationale**: Single .NET Framework WAP project (ContosoUniversity.csproj, net48); no dependency graph to manage. All-at-Once is fixed for single-project .NET Framework solutions per framework-migration planning rules.

### Execution Constraints
- Single atomic upgrade — all project changes applied together; validate full solution build after upgrade
- Side-by-side web migration: Scaffold task must complete and validate (builds, stub 200 response) before migrate starts
- Old Framework project remains live and deployable throughout entire migrate phase
- Migrate task will be broken into subtasks at execution time — load migrating-aspnet-framework-to-core skill
- Libraries in migrate task scope are handled in dependency order before web layer assets
- Reference cleanup (test projects, multi-targeting) is part of migrate, not a separate task
- Old project is NOT deleted by the agent — documented as post-upgrade step for user

### Side-by-Side Web Migration Constraints
- Scaffold task must complete and validate (builds, stub 200 response) before migrate starts
- Old Framework project remains live and deployable throughout entire migrate phase
- Migrate task will be broken into subtasks at execution time — load migrating-aspnet-framework-to-core skill
- Libraries in migrate task scope are handled in dependency order before web layer assets
- Reference cleanup (test projects, multi-targeting) is part of migrate, not a separate task
- Old project is NOT deleted by the agent — documented as post-upgrade step for user
