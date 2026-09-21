# 03.02-systemweb-adapters-scaffolding: Wire up System.Web Adapters hosting, migrate Global.asax/App_Start, BaseController, and bundling/static assets

## Objective
Stand up the System.Web Adapters bridge so the existing System.Web-based MVC surface can host on net10.0 in place (per the confirmed "Use System.Web Adapters" option — this is NOT a rewrite to ASP.NET Core MVC). Migrate the shared `BaseController` (used by all 7 controllers — must land before any per-controller subtask), convert `Global.asax.cs` application startup (`Feature.1000`), `App_Start/FilterConfig.cs` (`GlobalFilterCollection` → middleware, `Feature.0003`) and `App_Start/RouteConfig.cs` (`RouteCollection` → route mappings, `Feature.0002`), and replace `System.Web.Optimization` bundling (`App_Start/BundleConfig.cs`, `Feature.0001`) with a wwwroot/static-file-middleware + direct `<script>`/`<link>` tag approach across the 9 flagged views (`Views/Courses/Create.cshtml`, `Views/Courses/Edit.cshtml`, `Views/Departments/Create.cshtml`, `Views/Departments/Edit.cshtml`, `Views/Instructors/Create.cshtml`, `Views/Instructors/Edit.cshtml`, `Views/Shared/_Layout.cshtml`, `Views/Students/Create.cshtml`, `Views/Students/Edit.cshtml`).

This subtask must run after 03.01 (needs the project restoring/building on net10.0 with the `Microsoft.AspNetCore.SystemWebAdapters` package present) and before every per-controller subtask (03.05–03.10), which all depend on `BaseController` compiling and the hosting pipeline being in place.

## Scope
`Global.asax.cs`, `App_Start/FilterConfig.cs`, `App_Start/RouteConfig.cs`, `App_Start/BundleConfig.cs`, `Controllers/BaseController.cs`, a new `Program.cs` (net10.0 minimal-hosting entry point), `wwwroot/` setup, and the 9 views listed above (bundling references only — do not touch other controller-specific markup in those views).
Do NOT touch the `ConfigurationManager.ConnectionStrings` read that may remain in `Global.asax.cs` for connection-string bootstrapping — leave a clear marker/comment for subtask 03.03 to pick up after this subtask restructures the file, and do not remove that read yourself.

## Steps
1. Configure `Microsoft.AspNetCore.SystemWebAdapters` hosting: create `Program.cs` with the ASP.NET Core minimal-hosting model, register `services.AddSystemWebAdapters()` / `app.UseSystemWebAdapters()` (and any remote-app or in-process adapter set required — research current package docs for the exact API surface at the resolved version), so `HttpContext.Current`, `Session`, and MVC-5-style controller activation continue to work for the code migrated in later subtasks.
2. Port `Global.asax.cs` `Application_Start` logic (`Feature.1000`) into `Program.cs`: remove `GlobalFilterCollection` registration in favor of middleware/filter registration on the application builder (`Feature.0003`, replaces `App_Start/FilterConfig.cs`), and remove `RouteCollection` registration in favor of endpoint/route mapping on the application object (`Feature.0002`, replaces `App_Start/RouteConfig.cs`). Delete `FilterConfig.cs`/`RouteConfig.cs` once their logic is fully ported.
3. Migrate `Controllers/BaseController.cs` (2 `Api.0001` issues on `System.Web.Mvc.Controller.#ctor`) so it compiles against the adapters-bridged `Controller` base type. `SchoolContextFactory.Create()` and `NotificationService` wiring stay as-is — only the base-class/framework-API surface changes here.
4. Set up `wwwroot/` and static-file middleware (`app.UseStaticFiles()`); move/copy `Content/` and `Scripts/` assets as needed. Replace `App_Start/BundleConfig.cs` bundling logic and the `@Scripts.Render`/`@Styles.Render` calls in the 9 flagged views with direct `<script src="...">`/`<link rel="stylesheet" href="...">` tags. Delete `BundleConfig.cs` once no longer referenced.
5. Build the project. Expect remaining errors to be limited to per-controller `Api.0001` issues (owned by 03.05–03.10), `NotificationService.cs` (owned by 03.04), `SchoolContextFactory.cs`/config reads (owned by 03.03).

## Relevant skills
building-projects.

## Done when
- Project hosts via `Program.cs` + System.Web Adapters (no `Global.asax.cs` startup logic remains uncconverted; `Feature.1000`/`Feature.0002`/`Feature.0003` resolved).
- `BaseController` compiles with 0 API issues.
- Bundling (`Feature.0001`) is fully replaced across `BundleConfig.cs` and all 9 flagged views; `BundleConfig.cs` deleted.
- `dotnet build` shows no errors originating from `Global.asax.cs`, `FilterConfig.cs`, `RouteConfig.cs`, `BundleConfig.cs`, or `BaseController.cs`.
