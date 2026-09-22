# Plan: ContosoUniversity .NET 10 Upgrade

## Strategy
Single project, in-place rewrite from ASP.NET MVC 5 (.NET Framework 4.8) to ASP.NET Core MVC on `net10.0`. No side-by-side hosting needed (only one project, no shared consumers).

## Tasks
1. **Project file & SDK** — Convert `ContosoUniversity.csproj` to SDK-style (`Microsoft.NET.Sdk.Web`), target `net10.0`, replace `packages.config`/legacy references with `PackageReference` (EF Core 10.0.12, Microsoft.Data.SqlClient 7.1.0, Newtonsoft.Json 13.0.4, MSMQ.Messaging 1.0.4). Remove web-only legacy packages (WebGrease, Antlr, System.Web.*, Microsoft.AspNet.*).
2. **Host & configuration** — Add `Program.cs` (minimal hosting model), migrate `Web.config` connection string/app settings to `appsettings.json`, replace `ConfigurationManager` usage with `IConfiguration`/options pattern.
3. **MSMQ messaging migration** — Update `NotificationService` from `System.Messaging` to `MSMQ.Messaging`, move queue path to configuration via options pattern.
4. **Controllers** — Port all 6 controllers + `BaseController` to `Microsoft.AspNetCore.Mvc` (namespace swap, `HttpNotFound()`→`NotFound()`, `HttpStatusCodeResult`→`BadRequest()`, `JsonRequestBehavior` removed, `HttpPostedFileBase`→`IFormFile`, `Server.MapPath`→`IWebHostEnvironment`, `TryUpdateModel`→`TryUpdateModelAsync`).
5. **Routing & startup wiring** — Replace `RouteConfig`/`FilterConfig`/`BundleConfig`/`Global.asax` with `Program.cs` equivalents (`MapControllerRoute`, global filters, static files, DB seed call).
6. **Views & static assets** — Move `Content`/`Scripts` to `wwwroot`, remove `@Scripts.Render`/`@Styles.Render` in favor of direct `<script>`/`<link>` tags, add `_ViewImports.cshtml`, keep classic HTML helpers as-is, update `Error.cshtml` to drop `System.Web.Mvc.HandleErrorInfo`/`HttpContext.Current`.
7. **Cleanup & verification** — Remove `Global.asax(.cs)`, `App_Start`, `packages.config`, `Web.config`/`Web.*.config`, confirm no `System.Web` references remain, build the solution.

## Execution
Single execution task (`01-net10-upgrade`) — app is small enough (6 controllers, ~25 views, no auth/session/DI complexity) to migrate as one unit rather than decomposing into subtasks.
