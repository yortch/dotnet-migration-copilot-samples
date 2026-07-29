# Upgrade Plan — ContosoUniversity to .NET 10

## Selected Strategy
**All-At-Once** — The single ContosoUniversity project is upgraded via side-by-side web migration (new ASP.NET Core project alongside the legacy net48 WAP).
**Rationale**: 1 project (net48 WAP), .NET Framework single-project rule → All-at-Once. Side-by-side modifier injected because Project Approach = Side-by-side for the ASP.NET MVC Framework web project.

---

## Projects in Scope

| Project | Type | Current TFM | Target TFM | Approach |
|---------|------|-------------|------------|----------|
| ContosoUniversity.csproj | ASP.NET MVC WAP | net48 | net10.0 | Side-by-side (scaffold new Core project) |

---

## Tasks

### 01-prerequisites: Verify Prerequisites

Verify that all toolchain requirements for upgrading ContosoUniversity to .NET 10 are satisfied before any code changes begin. This includes confirming that the .NET 10 SDK is installed and that global.json (if present) does not pin an incompatible SDK version.

The assessment found the project uses packages.config for NuGet package management and a non-SDK-style csproj (Project.0001 flagged). Because ContosoUniversity is a side-by-side web migration target, the old project is intentionally kept in old-style format throughout migration — the new Core project created during scaffold is SDK-style. This task confirms that toolchain prerequisites are met and documents the starting state.

**Done when**: .NET 10 SDK is confirmed installed; global.json (if present) is compatible with net10.0; baseline solution state documented.

---

### 02-scaffold-contosouniversity: Scaffold New ASP.NET Core Project

Create a new ASP.NET Core web project (`ContosoUniversityCore`) in the same solution alongside the existing Framework project. This project will become the migration target for all web assets.

The scaffold task sets up the skeleton ASP.NET Core application: project file (net10.0, SDK-style), Program.cs with minimal hosting setup, appsettings.json (migrated from Web.config — standard connectionStrings and appSettings), YARP reverse proxy configured to forward unimplemented routes to the old Framework app, and initial package references (EF Core 10.0.x for Microsoft.EntityFrameworkCore.*, Microsoft.Data.SqlClient 7.0.x addressing the security vulnerability, Microsoft.AspNetCore.SystemWebAdapters for compatibility shims, and Microsoft.Windows.Compatibility for MSMQ/System.Messaging support).

Key signals from assessment: 22 binding redirects in Web.config (Document and Review selected — generate redirect inventory before removing), `System.Messaging` (MSMQ) usage in Services/NotificationService.cs (Windows Compatibility Pack will be added), `Microsoft.AspNet.Web.Optimization` is incompatible (replace with direct `<link>`/`<script>` tags in _Layout.cshtml), `Antlr` must be replaced with `Antlr4` (4.6.6+), `Microsoft.Identity.Client` is deprecated. The Data/SchoolContext DbContext and entity models will be copied to the new project and wired up to EF Core 10.

**Done when**: New `ContosoUniversityCore` project exists in the solution; solution builds (both old Framework project and new Core project); YARP proxy returns HTTP 200 for the root route forwarded to the old app; EF Core connection to the database is validated (migration or connection test).

---

### 03-migrate-contosouniversity: Migrate Web Assets to ASP.NET Core

Incrementally migrate all web assets — controllers, views, models, services, and configuration — from the old ASP.NET MVC Framework project to the new ASP.NET Core project. This is the primary execution task and will be broken into subtasks at execution time via the `migrating-aspnet-framework-to-core` skill.

Key migration areas identified in the assessment:
- **Global.asax.cs** (Feature.1000, Feature.0002, 3 binary + 8 source API issues): Convert application initialization code to Program.cs/Startup pattern; convert RouteCollection registrations to ASP.NET Core route mappings.
- **App_Start/RouteConfig.cs** (Feature.0002, 1 binary API issue): Remove and replace with attribute routing or endpoint routing in Program.cs.
- **Controllers/CoursesController.cs** (10 source API issues): Update to ASP.NET Core controller base class and APIs.
- **Data/SchoolContextFactory.cs** (6 source API issues): Update IDesignTimeDbContextFactory for EF Core.
- **Services/NotificationService.cs** (Feature.0008, 59 binary + 5 source API issues): Migrate System.Messaging (MSMQ) usage to .NET Core MSMQ via Windows Compatibility Pack.
- **Views (9 cshtml files)** (Feature.0001): Replace `@Styles.Render`/`@Scripts.Render` bundling calls with direct `<link>`/`<script>` tags.
- **Web.config binding redirects**: Generate inventory of 22 redirects, then remove (not needed in .NET Core).

Libraries in scope within this migrate task: None — ContosoUniversity has no dependent library projects. All non-web project files (PaginatedList.cs, Models, Data) are migrated as part of this task.

System.Web Adapters skill (`aspnet-system-web-adapters`) must be loaded as a standing context skill before subtask breakdown begins.

**Done when**: All controllers, views, models, and services are migrated to the Core project; old Framework project references in the solution can be removed (or are documented for user removal); solution builds without errors; all tests pass; no `System.Web` imports remain in the new project; binding redirects inventory documented and removed.

---

### 04-final-validation: Final Validation

Build the full solution, run all tests, and document any remaining recommendations. Confirm that the upgrade is complete and the new ASP.NET Core project functions correctly.

Verify: no build warnings in the migrated project; EF Core migrations run successfully; Microsoft.Data.SqlClient security vulnerability resolved (7.0.x); deferred items documented (old project removal, future EF Core migration from EF6 if applicable, nullable reference types enablement as a follow-on effort).

**Done when**: Solution builds with 0 errors and 0 warnings in ContosoUniversityCore; tests pass; final checklist of post-upgrade recommendations documented for the user.
