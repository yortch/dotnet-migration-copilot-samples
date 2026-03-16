# .NET 10 Upgrade

## Strategy
**All-At-Once** — Single project upgraded in one operation, organized by concern area.
**Rationale**: Single project (ContosoUniversity), clear migration path from ASP.NET MVC 5 to ASP.NET Core.

### Execution Constraints
- Single atomic upgrade — all concerns updated together
- Validate full solution build after upgrade
- No tier ordering needed — single project

## Preferences
- **Flow Mode**: Automatic
- **Commit Strategy**: After Each Task
- **Target Framework**: net10.0
- **Source Branch**: main
- **Working Branch**: upgrade-to-net10-coding-agent

## Decisions
- Use All-at-Once strategy (single project)
- Replace MSMQ NotificationService with in-memory ConcurrentQueue implementation
- Create a pull request when complete

## Custom Instructions
<!-- Task-specific overrides -->
