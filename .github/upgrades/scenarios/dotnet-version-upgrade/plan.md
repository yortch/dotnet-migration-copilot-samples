# ContosoUniversity .NET 10 Upgrade Plan

## Overview

**Target**: Upgrade ContosoUniversity from .NET Framework 4.8 (ASP.NET MVC 5) to .NET 10 (ASP.NET Core MVC)
**Scope**: 1 web application project, ~2,500 LOC across 16 affected files, Difficulty=High

### Selected Strategy
**All-At-Once** — Single project upgraded in one pass, with a side-by-side web migration modifier.
**Rationale**: 1 project, .NET Framework — All-at-Once is fixed per framework-migration rules. Side-by-side selected due to high difficulty (160 issues, System.Web, MSMQ) to allow incremental migration while keeping the old project live.

## Tasks

### 01-prerequisites: Verify SDK and toolchain readiness

Confirm that the .NET 10 SDK is installed and compatible with any `global.json` constraints in the repository. Validate that the `dotnet` CLI can target `net10.0` and that the working branch is clean. Record the installed SDK version and confirm no global.json pin blocks the upgrade.

**Done when**: `dotnet --version` reports a .NET 10 SDK; no global.json conflicts; working branch is clean and all changes committed.

---

### 02-scaffold-contoso: Scaffold new ASP.NET Core project alongside legacy project

Create a new ASP.NET Core MVC project (`ContosoUniversity.Core` or similar) in the solution alongside the existing `ContosoUniversity` (net48) project. Configure a YARP reverse proxy so the new Core project forwards unimplemented routes to the old Framework project, allowing incremental migration while both projects are live.

Set up the new project with:
- `net10.0` target framework, SDK-style `.csproj`
- `Microsoft.AspNetCore.SystemWebAdapters` for `HttpContext`/`HttpRequest`/`HttpResponse` compatibility shims
- `Microsoft.Windows.Compatibility` for System.Messaging (MSMQ) support
- YARP (`Yarp.ReverseProxy`) configured to forward all routes to the old project
- `appsettings.json` with connection string and notification queue path migrated from `web.config`
- `Program.cs` / `Startup.cs` with DI, EF Core, and routing bootstrapped
- Stub `HomeController` returning a 200 response to verify the scaffold builds and serves traffic

The old `ContosoUniversity` (net48) project must remain unchanged and fully buildable throughout this task. Assembly binding redirects from `web.config` should be documented (22 redirects) as a review artifact before being left out of the new project.

**Done when**: New ASP.NET Core project builds successfully (`dotnet build` with 0 errors); stub route returns HTTP 200; YARP is configured and routes forward to old project; old project still builds unchanged.

---

### 03-migrate-contoso: Migrate all web assets from legacy to ASP.NET Core project

Incrementally migrate all controllers, views, models, services, and configuration from `ContosoUniversity` (net48) to the new Core project. This task will be broken into subtasks at execution time — load the `migrating-aspnet-framework-to-core` skill before starting.

Key migration scope:
- **Controllers** (6): HomeController, StudentsController, CoursesController, InstructorsController, DepartmentsController, NotificationsController — convert from `System.Web.Mvc.Controller` to `Microsoft.AspNetCore.Mvc.Controller`; replace `ActionResult` with `IActionResult`
- **Data layer**: `SchoolContext` (EF Core 3.1.32 → 10.0.10), `DbInitializer`, `SchoolContextFactory` — update EF Core packages and address any API changes
- **Models** (9 + 3 ViewModels): straightforward; remove `System.ComponentModel.Annotations` package reference (now in-framework)
- **Services**: `NotificationService` using `System.Messaging` (MSMQ) — replace with `Microsoft.Windows.Compatibility` shim or document for later replacement
- **Views** (30 Razor views + `_Layout`): update `@using` directives, remove `System.Web.Mvc` references, update tag helpers, adapt bundling (replace `System.Web.Optimization` bundles with direct `<link>`/`<script>` tags)
- **Configuration**: `web.config` → `appsettings.json` / `IConfiguration`; remove all assembly binding redirects (22 total — auto-generated; document before removal)
- **Incompatible packages**: remove `Microsoft.AspNet.Web.Optimization` (no .NET Core equivalent); replace with direct CDN/static file references
- **Security**: upgrade `Microsoft.Data.SqlClient` from 2.1.4 → 7.0.2 (security vulnerability); upgrade `Microsoft.EntityFrameworkCore.*` from 3.1.32 → 10.0.10; upgrade `Newtonsoft.Json` from 13.0.3 → 13.0.4; remove deprecated `Microsoft.Identity.Client` 4.21.1 or upgrade

After all assets are migrated, remove the YARP proxy fallback and ensure all routes are served by the new Core project. Update any project/solution references to point to the new Core project.

**Done when**: New ASP.NET Core project serves all routes without forwarding to old project; `dotnet build` reports 0 errors and 0 warnings on new project; all EF Core packages on 10.0.10; security vulnerability in `Microsoft.Data.SqlClient` resolved; old project can be cleanly removed (but is NOT deleted — post-upgrade step for user).

---

### 04-final-validation: Final build validation and post-upgrade documentation

Run a full solution build and verify the new ASP.NET Core project compiles cleanly with 0 errors. Document any deferred work or known gaps (e.g., MSMQ → modern messaging migration, `Microsoft.Identity.Client` replacement). Commit all changes to the working branch.

**Done when**: Solution builds with 0 errors; all changed files committed to `copilot/upgrade-net10-cca-jul-30`; post-upgrade notes document remaining optional modernization steps.
