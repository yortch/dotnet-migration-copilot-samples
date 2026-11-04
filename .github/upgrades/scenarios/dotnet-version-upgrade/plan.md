# .NET 10 Upgrade Plan

## Overview
Migrate ContosoUniversity (.NET Framework 4.8, ASP.NET MVC 5) to ASP.NET Core MVC on net10.0.

## Tasks
### 01-project-and-startup: SDK-style project, Program.cs, appsettings
Replace legacy csproj/Global.asax/App_Start/Web.config with SDK-style Microsoft.NET.Sdk.Web csproj (EF Core SqlServer 10.x), Program.cs (DI, MVC, PascalCase JSON, static files incl. /Uploads, DB init), appsettings.json. Remove junk logs.

### 02-data-and-services: DbContext DI and in-memory notification service
Replace SchoolContextFactory with AddDbContext; replace MSMQ NotificationService with in-memory queue singleton.

### 03-controllers: Port controllers
System.Web.Mvc -> Microsoft.AspNetCore.Mvc, constructor DI, IFormFile uploads, TryUpdateModelAsync.

### 04-views-and-static: Views and wwwroot
_ViewImports, layout without bundles, move Content/Scripts to wwwroot, fix legacy helpers.

### 05-build-validate: Build with zero warnings, smoke test, commit
