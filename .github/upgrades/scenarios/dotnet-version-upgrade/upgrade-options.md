# Upgrade Options — ContosoUniversity

Assessment: 1 project (ContosoUniversity.csproj, net48 → net10.0), WAP with System.Web, 160 issues (High difficulty), 2 incompatible packages, 59 MSMQ API issues, 22 binding redirects

## Strategy

### Upgrade Strategy
Single .NET Framework WAP project — no dependency graph to manage; a single atomic upgrade pass is the appropriate strategy.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade the project in a single atomic pass. All project files, packages, and code changes applied together. No multi-targeting overhead. |
| Top-Down | Upgrade entry-point applications first, multi-targeting libraries temporarily. Adds complexity without benefit for a single-project solution. |

## Project Structure

### Project Approach
The project is an ASP.NET Framework MVC WAP (net48) with High difficulty and 160 issues. `System.Web` and `System.Web.Mvc` are architecturally incompatible with ASP.NET Core; multi-targeting is not viable.

| Value | Description |
|-------|-------------|
| **Side-by-side** (selected) | Creates a new ASP.NET Core project alongside the existing Framework project. Assets migrate incrementally while the old project stays live. Scaffold/migrate tasks injected into the plan. |
| In-place rewrite | Replaces the Framework web project entirely in one pass. Higher risk; appropriate only for small, low-complexity projects. Not recommended given High difficulty rating and 160 issues. |

## Compatibility

### Unsupported Packages
2 incompatible packages detected: `Microsoft.AspNet.Web.Optimization` (no .NET Core replacement in current form) and `Antlr` (replace with `Antlr4`).

| Value | Description |
|-------|-------------|
| **Resolve Inline** (selected) | Research and resolve each incompatible package within the same upgrade task. Small count (2 packages) makes inline resolution practical. |
| Defer Resolution | Generate minimal stubs for incompatible packages and create follow-up tasks. Appropriate for larger incompatible package counts (> 3). |

### Unsupported API Handling
Binary and source incompatible APIs detected across Global.asax.cs, RouteConfig.cs, NotificationService.cs, SchoolContextFactory.cs, and CoursesController.cs (96 mandatory + 29 potential API issues).

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve every API change in the same task. No deferred work, no stubs to clean up later. Default for All-at-Once strategy. |
| Defer Complex Changes | Apply simple replacements inline; generate stubs for complex changes and create resolution subtasks. Appropriate when Bottom-Up strategy is selected. |

### Windows Native APIs
`System.Messaging` (MSMQ) usage detected in `Services\NotificationService.cs` with 59 API issues. `System.Messaging` is Windows-only and has no direct cross-platform equivalent in .NET Core without additional packages.

| Value | Description |
|-------|-------------|
| **Windows Compatibility Pack** (selected) | Adds `Microsoft.Windows.Compatibility` package. Enables Windows APIs (including MSMQ clients) in .NET Core. App remains Windows-only until APIs are replaced. Defers cross-platform work. |
| No Compatibility Pack | Windows API build errors surface immediately; must be replaced with cross-platform alternatives before build succeeds. |

### System.Web Adapters
ASP.NET Framework MVC project with `System.Web` references. Side-by-side migration selected, requiring compatibility shims during incremental migration.

| Value | Description |
|-------|-------------|
| **Use System.Web Adapters** (selected) | Adds `Microsoft.AspNetCore.SystemWebAdapters` package. Provides compatibility shims for `HttpContext.Current`, `HttpRequest`, `HttpResponse`. Enables incremental migration; requires cleanup pass after migration completes. |
| Direct Migration to ASP.NET Core APIs | No adapter shims. All `System.Web` usage replaced immediately with native ASP.NET Core equivalents. More upfront work; appropriate only for small, low-complexity projects. |

## Modernization

### Assembly Binding Redirects
22 `<dependentAssembly>` entries in `Web.config`. Issues Binding.0006 (manual redirect conflicts with auto-generated version) and Binding.0007 (binding redirect forces version downgrade) detected — 6 occurrences each. Version downgrade redirects indicate real underlying version conflicts.

| Value | Description |
|-------|-------------|
| **Document and Review Before Removing** (selected) | Generates a report of all redirects and their purposes before removal. Recommended given > 10 redirects and version-downgrade redirects indicating real conflicts. |
| Remove Binding Redirects | Removes all redirects directly. .NET Core handles assembly resolution differently and does not need them. Use when redirects are known to be auto-generated boilerplate. |

### Nullable Reference Types
Target is `net10.0` and the project has High difficulty with 160 issues. Enabling nullable simultaneously would add compile-time warnings on top of an already demanding migration.

| Value | Description |
|-------|-------------|
| **Leave Disabled** (selected) | Does not enable nullable reference types. Maintains existing null handling. Enable as a distinct effort after migration stabilizes. |
| Enable Nullable Reference Types | Adds `<Nullable>enable</Nullable>` to project files. May require code updates to address warnings during migration. |
