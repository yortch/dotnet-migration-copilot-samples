# 01 progress
- Replaced legacy csproj with SDK-style Microsoft.NET.Sdk.Web (net10.0, EF Core SqlServer 10.0.0, nullable/implicit usings disabled).
- Added Program.cs (DI, MVC, PascalCase JSON, 10MB limits, static files + /Uploads, DB init) and appsettings.json.
- Removed Global.asax, App_Start, Web.config, Views\Web.config, packages.config, AssemblyInfo, SchoolContextFactory, junk logs, vsdoc scripts.
- Moved Content and Scripts into wwwroot.
- Not yet built; build validated in task 05.
