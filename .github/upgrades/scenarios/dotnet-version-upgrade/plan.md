# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade ContosoUniversity from .NET Framework 4.8 (ASP.NET MVC 5) to .NET 10 (ASP.NET Core MVC)
**Scope**: Single project, 83 files, EF Core 3.1→10, 45 NuGet packages, 147 assessment issues

### Selected Strategy
**All-at-Once** — Single project upgraded in one atomic operation.
**Rationale**: 1 project, no dependency graph to manage, in-place rewrite approach.

## Tasks

### 01-prerequisites: Verify SDK and toolchain readiness

Verify that the .NET 10 SDK is installed and compatible with the project. Check for any global.json files that might constrain the SDK version and update them if necessary.

**Done when**: .NET 10 SDK is confirmed available and any global.json constraints are compatible with net10.0.

---

### 02-sdk-style-conversion: Convert project to SDK-style format

Convert the legacy-format ContosoUniversity.csproj to SDK-style project format while remaining on .NET Framework 4.8. This is a structural change that must happen before the TFM upgrade. The project currently uses old-style csproj with explicit file includes, packages.config, and MSBuild imports. The conversion tool will handle packages.config → PackageReference migration.

**Done when**: ContosoUniversity.csproj is SDK-style format, packages.config is removed, project builds successfully on net48.

---

### 03-migrate-to-aspnet-core: Migrate from ASP.NET MVC 5 to ASP.NET Core on .NET 10

This is the core migration task. Change target framework from net48 to net10.0, upgrade all NuGet packages to their .NET 10 compatible versions, and rewrite the ASP.NET MVC 5 application to ASP.NET Core MVC. Key areas of work:

- Replace Global.asax.cs with Program.cs and ASP.NET Core startup configuration
- Convert all controllers from System.Web.Mvc to Microsoft.AspNetCore.Mvc (7 controllers: Home, Students, Courses, Instructors, Departments, Notifications, Base)
- Migrate Razor views from System.Web.Mvc to ASP.NET Core Razor (views for all entities plus shared layout)
- Convert Web.config to appsettings.json configuration
- Upgrade EF Core from 3.1 to 10.0.5, update DbContext and data access patterns
- Replace System.Web.Optimization bundling with direct HTML references or ASP.NET Core static file handling
- Convert route configuration from RouteCollection to ASP.NET Core endpoint routing
- Handle System.Messaging/MSMQ references (59 assessment issues)
- Remove incompatible packages (Microsoft.AspNet.Web.Optimization, Microsoft.AspNet.Mvc, etc.) and packages now included in the framework
- Fix 63 binary-incompatible and 29 source-incompatible API changes
- Address security vulnerability in Microsoft.Data.SqlClient (upgrade to 7.0.0)
- Remove assembly binding redirects, Web.config, Global.asax, and other Framework-specific files

**Done when**: Project targets net10.0, all packages are updated, all controllers and views use ASP.NET Core APIs, application builds with zero errors and zero warnings.

---

### 04-final-validation: Validate build and clean up

Final validation pass to ensure the fully migrated application builds cleanly. Remove any remaining Framework-specific artifacts, verify no build warnings exist, and confirm no deprecated or vulnerable packages remain.

**Done when**: Solution builds with zero errors and zero warnings, no security vulnerabilities in packages, all Framework-specific files cleaned up.
