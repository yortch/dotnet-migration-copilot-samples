# ContosoUniversity .NET 10 Upgrade Progress

## Overview

Upgrading ContosoUniversity from .NET Framework 4.8 (ASP.NET MVC 5) to .NET 10 (ASP.NET Core MVC) using an All-at-Once strategy with a side-by-side web migration modifier. A new ASP.NET Core project is scaffolded alongside the existing Framework project, and web assets are migrated incrementally.

**Progress**: 2/4 tasks complete <progress value="50" max="100"></progress> 50%

## Tasks

- ✅ 01-prerequisites: Verify SDK and toolchain readiness ([Content](tasks/01-prerequisites/task.md), [Progress](tasks/01-prerequisites/progress-details.md))
- ✅ 02-scaffold-contoso: Scaffold new ASP.NET Core project alongside legacy project ([Content](tasks/02-scaffold-contoso/task.md), [Progress](tasks/02-scaffold-contoso/progress-details.md))
- 🔲 03-migrate-contoso: Migrate all web assets from legacy to ASP.NET Core project
- 🔲 04-final-validation: Final build validation and post-upgrade documentation
