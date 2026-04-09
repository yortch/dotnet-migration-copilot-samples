# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade the ContosoUniversity ASP.NET MVC 5 sample from .NET Framework 4.8 to ASP.NET Core on .NET 10.
**Scope**: Single web project, legacy project system, packages.config-based dependencies, System.Web APIs, MSMQ integration, and EF Core package updates.

## Tasks

### 01-prerequisites: Verify upgrade prerequisites

Confirm the available SDK, capture the current restore/build baseline, and inventory the parts of the legacy app that shape the rewrite. This task covers the project file, configuration, package layout, and the framework-specific services called out by the assessment so the upgrade starts from a known state.

Use this task to document the existing build failure on the classic web targets import, confirm there are no test projects to run, and record the files that will drive the migration work: the legacy csproj, Global.asax startup path, MVC route/bundle registration, Web.config settings, and the notification queue service.

**Done when**: The current toolchain state and baseline build result are captured in the task artifacts, the affected files and technologies are documented, and the upgrade can proceed with no missing prerequisite information.

---

### 02-sdk-style-conversion: Convert the web project to SDK-style on its current framework

Convert ContosoUniversity from the legacy web application project format to an SDK-style project while it still targets .NET Framework 4.8. This isolates project-system changes from the later framework rewrite and removes `packages.config` in favor of PackageReference so package updates can be managed cleanly.

This task focuses on the single `ContosoUniversity.csproj` file and the package-management surface identified in the assessment. It must preserve the application's source layout and keep the project buildable enough for the migration to continue, even if the classic web tooling import disappears as part of the conversion.

**Done when**: `ContosoUniversity.csproj` is SDK-style, `packages.config` is no longer authoritative, legacy import plumbing is removed, and the project is ready for a .NET 10 rewrite.

---

### 03-web-app-upgrade: Rewrite the application for ASP.NET Core on .NET 10

Upgrade the application itself to .NET 10 by replacing the legacy ASP.NET MVC runtime with ASP.NET Core MVC, updating packages, and fixing the framework API changes reported by the assessment. This includes startup conversion from `Global.asax` to `Program.cs`, routing and static asset changes, configuration migration from `Web.config` to `appsettings.json`, EF Core package updates, and direct replacement of `System.Web` APIs.

This task also covers the unsupported and deprecated package issues plus the code paths behind the highest-risk API findings, especially the notification queue implementation, file upload handling, request/response abstractions, and Razor view helpers. All code changes should land in a single compilable ASP.NET Core project that targets `net10.0`.

**Done when**: The project targets `net10.0`, the ASP.NET Core hosting model replaces the legacy MVC startup path, incompatible packages and legacy framework references are removed or replaced, and the solution builds warning-free on the upgraded stack.

---

### 04-final-validation: Validate the upgraded solution and finalize workflow artifacts

Run the upgraded solution through restore and build validation, address any remaining warnings in modified code, and reconcile the workflow artifacts with the final outcome. This task closes the loop on the migration by confirming that the project is in a clean, supportable state and that the workflow records show what happened.

Use this task to verify the final project structure, document any intentionally deferred follow-up work, and update the scenario progress files so the session accurately records success, failures, or skips for every task.

**Done when**: Final restore/build validation succeeds with no warnings in modified projects, workflow artifacts are updated to match the executed work, and the task list reflects the completed upgrade.
