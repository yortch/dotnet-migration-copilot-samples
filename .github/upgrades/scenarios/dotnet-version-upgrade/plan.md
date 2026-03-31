# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade ContosoUniversity from .NET Framework 4.8 (ASP.NET MVC 5) to .NET 10 (ASP.NET Core MVC)
**Scope**: 1 project, ~15 source files, legacy project format with packages.config

### Selected Strategy
**All-At-Once** — Single project upgraded in one operation.
**Rationale**: 1 project, straightforward ASP.NET MVC 5 → ASP.NET Core migration.

## Tasks

### 01-convert-project: Convert Project to SDK-Style and Update Target Framework

Convert the legacy .csproj to SDK-style format, update the target framework to net10.0, and update all NuGet package references. Remove packages that are now included in the framework (Microsoft.AspNet.Mvc, Microsoft.AspNet.Razor, Microsoft.AspNet.WebPages, etc.). Update EF Core and other packages to their .NET 10 compatible versions. Remove packages.config.

**Done when**: Project file is SDK-style with `<TargetFramework>net10.0</TargetFramework>`, all NuGet packages are updated or removed, and `dotnet restore` succeeds.

---

### 02-migrate-aspnet-core: Migrate to ASP.NET Core

Migrate the application from ASP.NET MVC 5 (System.Web) to ASP.NET Core MVC. This includes:
- Create Program.cs with ASP.NET Core host builder, replacing Global.asax
- Convert controllers from System.Web.Mvc.Controller to Microsoft.AspNetCore.Mvc.Controller
- Update controller patterns (HttpStatusCodeResult → StatusCode, HttpNotFound → NotFound, etc.)
- Replace System.Web.Optimization bundling with static file references in layout
- Convert routing from RouteConfig to ASP.NET Core endpoint routing
- Update Razor views to use ASP.NET Core tag helpers and patterns
- Create _ViewImports.cshtml with tag helper imports
- Replace System.Messaging (MSMQ) with an in-memory notification approach
- Update SchoolContext and data access for ASP.NET Core DI patterns
- Remove legacy files (Global.asax, Web.config, App_Start, etc.)

**Done when**: Application compiles and builds successfully with `dotnet build` producing zero errors.

---

### 03-fix-warnings: Fix Build Warnings

Address any remaining build warnings to ensure a clean build. This may include fixing nullable reference type warnings, obsolete API usage, or other compiler warnings.

**Done when**: `dotnet build` produces zero errors and zero warnings.
