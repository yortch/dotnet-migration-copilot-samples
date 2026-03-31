# .NET 10 Upgrade Plan

## Overview

**Target**: Upgrade ContosoUniversity from .NET Framework 4.8 (ASP.NET MVC 5) to .NET 10 (ASP.NET Core)
**Scope**: 1 project, ~3,400 LOC, 56 code files — full framework migration

### Selected Strategy
**All-At-Once** — Single project upgraded in one operation, organized by concern area.
**Rationale**: Single project, clear migration path from ASP.NET MVC 5 to ASP.NET Core.

## Tasks

### 01-sdk-conversion: Convert project to SDK-style format

Convert the legacy .csproj to SDK-style format. The current project uses the classic MSBuild format with explicit file includes, assembly references, and packages.config. Convert to the modern SDK-style format that ASP.NET Core requires.

**Done when**: Project file is SDK-style format with `Microsoft.NET.Sdk.Web` SDK.

---

### 02-framework-packages: Update target framework and NuGet packages

Update TargetFramework to net10.0 and replace/update all NuGet packages. Remove packages that are now part of the framework (Microsoft.AspNet.Mvc, System.Web references, etc.). Update EF Core packages from 3.1.32 to 10.0.x. Update Microsoft.Extensions.* packages to 10.0.x. Replace incompatible packages with modern equivalents.

**Done when**: Project targets net10.0, all package references are compatible with .NET 10, no incompatible or deprecated packages remain.

---

### 03-aspnet-core-bootstrap: Create ASP.NET Core entry point and migrate configuration

Create Program.cs with ASP.NET Core minimal hosting. Migrate connection strings and app settings from Web.config to appsettings.json. Configure services (DbContext, MVC, static files). Remove Global.asax, Global.asax.cs, Web.config, App_Start folder, packages.config, and other legacy files. Configure routing to match existing URL patterns.

**Done when**: Program.cs configures all services and middleware. appsettings.json contains connection string and app settings. Legacy startup files removed.

---

### 04-controllers-views: Migrate controllers and views to ASP.NET Core

Migrate all controllers from System.Web.Mvc to Microsoft.AspNetCore.Mvc. Replace HttpPostedFileBase with IFormFile for file uploads. Update view imports and layout for ASP.NET Core Razor. Create _ViewImports.cshtml with tag helpers. Update all Razor views to use ASP.NET Core syntax (tag helpers, etc.). Migrate PaginatedList to use async EF Core methods.

**Done when**: All controllers inherit from Microsoft.AspNetCore.Mvc.Controller. All views use ASP.NET Core Razor syntax. File upload uses IFormFile.

---

### 05-services-infrastructure: Migrate services and data infrastructure

Replace MSMQ-based NotificationService with an in-memory implementation (ConcurrentQueue-based) since MSMQ is not available in .NET Core. Update SchoolContextFactory to use ASP.NET Core configuration instead of ConfigurationManager. Update SchoolContext for EF Core 10 compatibility. Ensure DbInitializer works with the new DI container.

**Done when**: NotificationService works without System.Messaging. SchoolContextFactory uses modern configuration. Data layer is fully compatible with EF Core 10.

---

### 06-build-validation: Build validation and error fixes

Build the solution, fix any remaining compilation errors, and ensure the application is ready. Address any API breaking changes between .NET Framework 4.8 and .NET 10.

**Done when**: Solution builds with 0 errors. All compilation issues resolved.
