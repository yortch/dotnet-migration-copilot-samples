# Upgrade Options — ContosoUniversity

Assessment: 1 .NET Framework MVC web project, non-SDK-style, 40 package issues, 92 API issues, System.Web and MSMQ usage detected.

## Strategy

### Upgrade Strategy
The solution has a single `net48` project and no inter-project dependency graph, so the recommended strategy is a single coordinated upgrade pass.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade all projects simultaneously in a single atomic pass. |
| Bottom-Up | Upgrade leaf-node libraries first, then work upward tier by tier. |
| Top-Down | Upgrade entry-point applications first and multi-target shared libraries during the transition. |

## Project Structure

### Project Approach
ContosoUniversity is a single ASP.NET MVC 5 project with six controllers, no companion libraries, and no requirement to keep the old sample live during migration.

| Value | Description |
|-------|-------------|
| **Web Projects = In-place rewrite** (selected) | Replace the existing ASP.NET Framework web project with an ASP.NET Core project in one pass. |
| Web Projects = Side-by-side | Create a new ASP.NET Core project beside the existing one and migrate incrementally. |

## Compatibility

### Unsupported Packages
The assessment flagged two incompatible packages, and their replacements can be resolved as part of the main rewrite without introducing stub work.

| Value | Description |
|-------|-------------|
| **Resolve Inline** (selected) | Research and replace incompatible packages inside the same task with no deferred work. |
| Defer Resolution | Stub or temporarily remove incompatible packages and resolve them in follow-up tasks. |
| Compatibility Mode | Keep .NET Framework references temporarily and suppress compatibility warnings. |

### Unsupported API Handling
The project has many framework API changes, but they are concentrated in one web application and are best resolved directly while the ASP.NET Core rewrite is underway.

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve every API change in the same task, including complex ones. |
| Defer Complex Changes | Apply simple replacements now and create stub-backed follow-up tasks for complex APIs. |

### System.Web Adapters
`HttpContext.Current` usage is limited and the project is being rewritten in place, so a compatibility shim would add cleanup work without much benefit.

| Value | Description |
|-------|-------------|
| **Direct Migration to ASP.NET Core APIs** (selected) | Replace `System.Web` usage immediately with native ASP.NET Core equivalents. |
| Use System.Web Adapters | Add compatibility shims for incremental `System.Web` migration. |

## Modernization

### Configuration Migration
The existing `Web.config` contains standard connection strings, a few app settings, and IIS upload limits, which can be migrated directly to ASP.NET Core configuration.

| Value | Description |
|-------|-------------|
| **Auto-migrate to .NET Core Configuration** (selected) | Convert standard configuration to `appsettings.json` and `IConfiguration` during the rewrite. |
| Manual Migration with Mapping Document | Produce a mapping document before moving complex configuration. |

### Assembly Binding Redirects
The current binding redirects are standard ASP.NET boilerplate and should be removed when the project moves to .NET 10.

| Value | Description |
|-------|-------------|
| **Remove Binding Redirects** (selected) | Remove all binding redirects because .NET Core resolves assemblies differently. |
| Document and Review Before Removing | Inventory redirects before removing them. |

### Nullable Reference Types
The migration already includes a project-system rewrite, package updates, and API changes, so enabling nullable is better left for a later focused effort.

| Value | Description |
|-------|-------------|
| **Leave Disabled** (selected) | Keep nullable annotations disabled during the framework migration. |
| Enable Nullable Reference Types | Turn on nullable analysis and fix resulting warnings as part of the upgrade. |
