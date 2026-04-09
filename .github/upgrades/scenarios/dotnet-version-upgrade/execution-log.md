
## [2026-04-09 15:29] 01-prerequisites

Captured the ContosoUniversity upgrade baseline. Documented the legacy project shape, startup/configuration entry points, and the MSMQ/bundling areas that will need replacement. Verified that no test projects exist and recorded the current build failure: `dotnet build` on the legacy solution stops with MSB4019 because `Microsoft.WebApplication.targets` is unavailable until the project is converted away from the classic web application format.


## [2026-04-09 15:33] 02-sdk-style-conversion

Converted `ContosoUniversity.csproj` to SDK-style and removed `packages.config`, but the interim net48 build remains blocked. `dotnet build ContosoUniversity.csproj -warnaserror` now restores through the SDK-style project system, yet fails on the vulnerable `Microsoft.Data.SqlClient` package and several downgrade conflicts (`Microsoft.Bcl.AsyncInterfaces`, `Microsoft.Extensions.Caching.Memory`, `Microsoft.Extensions.Logging.Abstractions`, `System.Diagnostics.DiagnosticSource`, and `System.Memory`). These blockers were documented and will be resolved as part of the full .NET 10 rewrite.


## [2026-04-09 15:38] 03.01-bootstrap-config

Rebuilt the project bootstrap for ASP.NET Core. Replaced the interim SDK-style net48 project file with a minimal `net10.0` web project, added `Program.cs` and `appsettings.json`, and removed the legacy MVC startup/configuration files (`Global.asax*`, `App_Start/*`, `Web.config`, and `Views/Web.config`). To support the remaining migration subtasks, the still-unmigrated controllers and notification service were temporarily excluded from compilation. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:39] 03.02-static-assets

Migrated the static asset pipeline away from System.Web bundling. `Program.cs` now serves the existing `Content`, `Scripts`, and `Uploads` directories as static files, `_Layout.cshtml` uses direct stylesheet/script tags, and the affected CRUD views now reference the jQuery validation scripts explicitly. Added `_ViewImports.cshtml` for ASP.NET Core Razor imports and validated with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:40] 03.03-notification-service

Replaced the legacy MSMQ dependency with a database-backed notification service. `NotificationService` now stores notifications through `SchoolContext`, exposes unread/read operations for the later controller work, and is registered in ASP.NET Core DI. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:41] 03.04-base-controller

Migrated the shared controller infrastructure to ASP.NET Core. `BaseController` now inherits from `Microsoft.AspNetCore.Mvc.Controller`, is compiled into the net10 project, and receives `SchoolContext` plus `NotificationService` through DI instead of manually instantiating them. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:43] 03.05-home-controller

Migrated `HomeController` to ASP.NET Core MVC and reintroduced it into the net10 build. The shared error view now uses `ErrorViewModel` with the ASP.NET Core request identifier instead of `HandleErrorInfo`, and the legacy unauthorized route name was preserved via `ActionName`. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:45] 03.06-courses-controller

Migrated `CoursesController` to ASP.NET Core MVC and added it back to the net10 project. The controller now uses `IFormFile` plus `IWebHostEnvironment` for teaching-material uploads instead of `HttpPostedFileBase` and `Server.MapPath`, while keeping the existing CRUD flow and notification hooks. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:46] 03.07-departments-controller

Migrated `DepartmentsController` to ASP.NET Core MVC and added it back to the net10 project. The CRUD actions now use ASP.NET Core result helpers and drop-down handling while retaining the existing EF Core concurrency resolution path. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:47] 03.08-instructors-controller

Migrated `InstructorsController` to ASP.NET Core MVC and added it back to the net10 project. The edit flow now uses `TryUpdateModelAsync` while keeping the assigned-course selection and notification behavior intact. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:47] 03.09-notifications-controller

Migrated `NotificationsController` to ASP.NET Core MVC and added it back to the net10 project. The JSON endpoints now call the database-backed `NotificationService` unread/read methods instead of draining an MSMQ queue. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.


## [2026-04-09 15:49] 03.10-students-controller

Migrated `StudentsController` to ASP.NET Core MVC and added it back to the net10 project. The search/sort/pagination flow now uses ASP.NET Core action patterns while preserving the existing enrollment-date validation and notification behavior. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.

