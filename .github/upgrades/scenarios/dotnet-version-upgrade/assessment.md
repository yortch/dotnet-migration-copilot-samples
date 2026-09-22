# Assessment: ContosoUniversity .NET 10 Upgrade

## Solution Overview
- Single project solution: `ContosoUniversity\ContosoUniversity.csproj`, legacy (non-SDK-style) csproj.
- Current target: .NET Framework 4.8, `OutputType=Library` (ASP.NET MVC 5 web application project).
- Stack: ASP.NET MVC 5 (`System.Web.Mvc`), Razor views, EF Core 3.1 (already using EF Core instead of EF6, referenced via legacy `<Reference>`/`packages.config` instead of `<PackageReference>`).

## Feature Inventory (drives satellite loading / migration scope)
- **Controllers**: 6 controllers (`HomeController`, `StudentsController`, `CoursesController`, `InstructorsController`, `DepartmentsController`, `NotificationsController`) + abstract `BaseController`. Plain MVC controllers, no Web API controllers.
- **Auth**: None. No `FormsAuthentication`, `Membership`, `[Authorize]`, or OWIN. `FilterConfig` explicitly has the auth filter commented out.
- **Session/TempData**: `TempData` used once (`StudentsController.DeleteConfirmed`); no `Session` usage.
- **DI container**: None (no Autofac/Ninject/Unity). Controllers new-up `SchoolContext`/`NotificationService` directly via `BaseController`.
- **Routing**: Single conventional route in `RouteConfig.cs` (`{controller}/{action}/{id}`), no attribute routing/areas.
- **Global.asax**: `Application_Start` wires `AreaRegistration`, `FilterConfig`, `RouteConfig`, `BundleConfig`, and seeds the DB via `DbInitializer`.
- **Bundling**: `BundleConfig.cs` + `@Scripts.Render`/`@Styles.Render` in `_Layout.cshtml` and several views' `@section Scripts`.
- **File upload**: `CoursesController` uses `HttpPostedFileBase` + `Server.MapPath` to save teaching-material images under `Uploads/TeachingMaterials`.
- **Model binding**: `[Bind(Include = "...")]` attributes; `InstructorsController.Edit` uses legacy `TryUpdateModel(model, prefix, string[] includeProperties)`.
- **MSMQ messaging**: `Services/NotificationService.cs` uses `System.Messaging` (Windows-only, not supported on .NET Core) — requires migration to `MSMQ.Messaging` NuGet package per the `migrating-to-msmq-messaging` satellite.
- **Config**: `Web.config` `<connectionStrings>` (`DefaultConnection`) and `<appSettings>` (`NotificationQueuePath`, webpages flags) read via `ConfigurationManager`.
- **Views**: Classic Razor using `Html.ActionLink`, `Html.EditorFor/DisplayFor/LabelFor/ValidationMessageFor`, `Html.BeginForm`, `Html.DropDownList`, `Html.Raw` — all still supported by ASP.NET Core's `IHtmlHelper`, so views need minimal changes (mainly removing bundling calls).

## Packages Requiring Update
| Package | Current | Target |
|---|---|---|
| Microsoft.EntityFrameworkCore(.SqlServer/.Relational/.Abstractions) | 3.1.32 | 10.0.12 |
| Microsoft.Data.SqlClient(.SNI) | 2.1.4 | 7.1.0 |
| Newtonsoft.Json | 13.0.3 | 13.0.4 |
| Microsoft.Extensions.* (DI/Configuration/Logging/Caching/Options) | 3.1.32 | provided by ASP.NET Core 10 shared framework (remove explicit refs) |
| System.Messaging (implicit via GAC) | n/a | `MSMQ.Messaging` 1.0.4 |
| Web-only packages (`Microsoft.AspNet.Mvc`, `Microsoft.AspNet.WebPages`, `Microsoft.AspNet.Razor`, `WebGrease`, `Antlr`, `Microsoft.AspNet.Web.Optimization`, `Microsoft.CodeDom.Providers.DotNetCompilerPlatform`, `Microsoft.Web.Infrastructure`) | various | removed — replaced by ASP.NET Core MVC |

## Risks
- MSMQ is Windows-only; this limitation carries over unchanged (was already Windows-only under .NET Framework).
- `TryUpdateModel` (sync, string-array include list) must become `TryUpdateModelAsync` (expression-based include list) — behavior-preserving but requires the action to become `async Task<ActionResult>`.
- File upload code (`Server.MapPath`, `HttpPostedFileBase.SaveAs`) needs `IWebHostEnvironment` + `IFormFile` equivalents.
- Bundling/minification has no built-in ASP.NET Core replacement; static assets will be served unbundled from `wwwroot`, preserving behavior but not the bundling optimization (out of scope for this upgrade).

## Conclusion
Scope is a single small MVC 5 project with no auth/session/DI complexity. Migration proceeds in-place: SDK-style project targeting `net10.0`, `Program.cs` minimal hosting model, controllers/views ported to ASP.NET Core MVC, MSMQ messaging updated, and static assets moved to `wwwroot`.
