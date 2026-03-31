# .NET Version Upgrade

## Strategy
**Selected**: All-At-Once
**Rationale**: Single project solution (ContosoUniversity), migrating from .NET Framework 4.8 (ASP.NET MVC 5) to .NET 10. All work targets one project — no tier ordering needed.

### Execution Constraints
- Single atomic upgrade — all changes applied to ContosoUniversity project
- Convert to SDK-style project first, then update TFM and packages
- Migrate ASP.NET MVC 5 → ASP.NET Core MVC patterns (Program.cs, controllers, views)
- Validate full solution build after upgrade
- Address all build warnings

## Preferences
- **Flow Mode**: Automatic
- **Commit Strategy**: After Each Task
- **Pace**: Standard
- **Target Framework**: net10.0
- **Source Branch**: copilot/upgrade-to-net10-cca-dryrun
- **Working Branch**: copilot/upgrade-to-net10-cca-dryrun

## Decisions
- Use All-at-Once strategy for single project upgrade
- Migrate from ASP.NET MVC 5 to ASP.NET Core MVC
- Replace System.Messaging (MSMQ) with in-memory queue alternative
- Replace System.Web.Optimization bundling with static file references

## Custom Instructions
<!-- Task-specific overrides -->
