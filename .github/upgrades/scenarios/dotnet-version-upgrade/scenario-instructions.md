# Scenario Instructions: .NET Version Upgrade

## Goal
Upgrade the ContosoUniversity project (ASP.NET MVC 5 / .NET Framework 4.8, System.Web) to .NET 10.

## Parameters
- Target Framework: net10.0
- Project(s) in scope: ContosoUniversity\ContosoUniversity.csproj (only project in the solution)
- Project Approach: In-place rewrite (single project, no side-by-side needed - no other host consumes this app)
- Flow Mode: Automatic
- Source control: working branch `copilot/upgrade-net10-cca-sep-22` (already checked out), commit via report_progress

## User Preferences
- User requested: "Upgrade ContosoUniversity project to .NET 10 and commit changes into upgrade-net10-cca-sep-22" — proceed end-to-end, commit results to the existing branch.

## Decisions
- No authentication, session, OWIN, or third-party DI container present — simplifies DI/auth migration steps (skipped).
- `System.Messaging` (MSMQ) usage in `NotificationService` migrated to `MSMQ.Messaging` NuGet package per `migrating-to-msmq-messaging` satellite skill.
- Bundling (`System.Web.Optimization`) removed; static assets served directly from `wwwroot` with individual `<script>`/`<link>` tags (no bundler introduced, per minimal-change principle).
- Classic HTML helpers (`Html.ActionLink`, `Html.EditorFor`, `Html.BeginForm`, etc.) retained as-is since ASP.NET Core still supports them, avoiding unnecessary view rewrites to Tag Helpers.
