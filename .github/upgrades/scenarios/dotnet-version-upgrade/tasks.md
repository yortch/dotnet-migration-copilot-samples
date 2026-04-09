# .NET Version Upgrade Progress

## Overview

Upgrade the ContosoUniversity sample from ASP.NET MVC 5 on .NET Framework 4.8 to ASP.NET Core on .NET 10 using an all-at-once, in-place rewrite. The workflow separates prerequisite capture, project-system conversion, application rewrite, and final validation so the migration stays traceable.

**Progress**: 1/4 tasks complete <progress value="25" max="100"></progress> 25%

## Tasks

- ✅ 01-prerequisites: Verify upgrade prerequisites ([Content](tasks/01-prerequisites/task.md), [Progress](tasks/01-prerequisites/progress-details.md))
- 🔲 02-sdk-style-conversion: Convert the web project to SDK-style on its current framework
- 🔲 03-web-app-upgrade: Rewrite the application for ASP.NET Core on .NET 10
- 🔲 04-final-validation: Validate the upgraded solution and finalize workflow artifacts
