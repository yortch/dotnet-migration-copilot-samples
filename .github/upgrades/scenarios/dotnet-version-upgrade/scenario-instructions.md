# Scenario Instructions — ContosoUniversity .NET 10 Upgrade

## Goal
Upgrade ContosoUniversity from .NET Framework 4.8 (ASP.NET MVC 5) to .NET 10 (ASP.NET Core MVC).

## Parameters

### Target Framework
- **Target**: net10.0 (LTS — support ends Nov 2028)
- **Current**: net48 (ASP.NET MVC 5 / System.Web)

### Source Control
- **Working Branch**: copilot/upgrade-net10-cca-jul-30
- **Commit Strategy**: After Each Task

## Strategy
**Selected**: All-at-Once (single project, .NET Framework — fixed per framework-migration rules)
**Rationale**: Single .NET Framework 4.8 web project; no dependency graph to manage. Side-by-side web migration modifier applied.

### Execution Constraints
- Single atomic upgrade pass; validate full solution build after each major task
- Side-by-side: scaffold must complete and validate (builds, stub 200 response) before migrate starts
- Old Framework project remains live and deployable throughout entire migrate phase
- Migrate task will be broken into subtasks at execution time — load migrating-aspnet-framework-to-core skill
- Reference cleanup is part of migrate task, not a separate task
- Old project is NOT deleted by the agent — documented as post-upgrade step for user

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Project Structure
- Project Approach: Side-by-side (Web Projects)

### Compatibility
- Unsupported Packages: Resolve Inline (1 incompatible package)
- Unsupported API Handling: Fix Inline
- Windows Native APIs: Windows Compatibility Pack
- System.Web Adapters: Use System.Web Adapters
  Skill: aspnet-system-web-adapters

### Modernization
- Configuration Migration: Auto-migrate to .NET Core Configuration
- Assembly Binding Redirects: Document and Review Before Removing
- Nullable Reference Types: Leave Disabled

## User Preferences

### Execution Style
- Flow mode: **Automatic** (inferred from problem statement — proceed without pausing between stages)
