
## [2026-04-09 15:29] 01-prerequisites

Captured the ContosoUniversity upgrade baseline. Documented the legacy project shape, startup/configuration entry points, and the MSMQ/bundling areas that will need replacement. Verified that no test projects exist and recorded the current build failure: `dotnet build` on the legacy solution stops with MSB4019 because `Microsoft.WebApplication.targets` is unavailable until the project is converted away from the classic web application format.


## [2026-04-09 15:33] 02-sdk-style-conversion

Converted `ContosoUniversity.csproj` to SDK-style and removed `packages.config`, but the interim net48 build remains blocked. `dotnet build ContosoUniversity.csproj -warnaserror` now restores through the SDK-style project system, yet fails on the vulnerable `Microsoft.Data.SqlClient` package and several downgrade conflicts (`Microsoft.Bcl.AsyncInterfaces`, `Microsoft.Extensions.Caching.Memory`, `Microsoft.Extensions.Logging.Abstractions`, `System.Diagnostics.DiagnosticSource`, and `System.Memory`). These blockers were documented and will be resolved as part of the full .NET 10 rewrite.


## [2026-04-09 15:38] 03.01-bootstrap-config

Rebuilt the project bootstrap for ASP.NET Core. Replaced the interim SDK-style net48 project file with a minimal `net10.0` web project, added `Program.cs` and `appsettings.json`, and removed the legacy MVC startup/configuration files (`Global.asax*`, `App_Start/*`, `Web.config`, and `Views/Web.config`). To support the remaining migration subtasks, the still-unmigrated controllers and notification service were temporarily excluded from compilation. Validation succeeded with `dotnet build ContosoUniversity.csproj -warnaserror`.

