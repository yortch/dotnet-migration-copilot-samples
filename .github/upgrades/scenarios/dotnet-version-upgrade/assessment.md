# Assessment: ContosoUniversity -> net10.0

Single project: `ContosoUniversity\ContosoUniversity.csproj` (.NET Framework 4.8, ASP.NET MVC 5, legacy csproj, packages.config). EF Core 3.1.32 already in use.

## Blockers / required rewrites
- Legacy csproj -> SDK-style `Microsoft.NET.Sdk.Web`, net10.0; EF Core 10 SqlServer.
- Global.asax, App_Start (Bundle/Filter/Route config), Web.config, Views\Web.config -> Program.cs, appsettings.json, _ViewImports.cshtml.
- System.Web.Mvc -> Microsoft.AspNetCore.Mvc (ActionResult->IActionResult, HttpNotFound, HttpStatusCodeResult, Bind(Include), TryUpdateModel, JsonRequestBehavior, HttpPostedFileBase, Server.MapPath).
- System.Messaging (MSMQ) in NotificationService: not available -> in-memory queue singleton.
- ConfigurationManager / SchoolContextFactory -> DI (AddDbContext, IConfiguration).
- BaseController creates its own DbContext -> constructor injection.
- Razor views: @Html helpers -> tag helpers/compatible helpers, bundles -> wwwroot links.
- Static content (Content, Scripts) -> wwwroot; /Uploads served as static files.
- Remove tracked build logs.
