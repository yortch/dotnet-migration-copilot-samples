# Upgrade Options — ContosoUniversity

Assessment: 1 project (net48 / ASP.NET MVC 5), 160 issues (96 mandatory), Difficulty=High; System.Web, MSMQ, 22 binding redirects, 1 incompatible package.

## Strategy

### Upgrade Strategy
Single .NET Framework project — All-at-Once is fixed per framework migration rules.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade the single project in one pass; no dependency graph to manage. |

## Project Structure

### Project Approach
Web project with System.Web + High difficulty assessment (160 issues) indicates high-risk breaking changes; side-by-side is safer than in-place rewrite.

| Value | Description |
|-------|-------------|
| **Side-by-side** (selected) | Create a new ASP.NET Core project alongside the old Framework project; migrate assets incrementally while old project stays live. Injects scaffold + migrate tasks. |
| In-place rewrite | Replace the Framework web project entirely in one pass. Higher risk; faster for small low-complexity projects. |

## Compatibility

### Unsupported Packages
1 incompatible package found (Microsoft.AspNet.Web.Optimization); small enough to resolve inline.

| Value | Description |
|-------|-------------|
| **Resolve Inline** (selected) | Research and replace the incompatible package within the same task; no deferred work. |
| Defer Resolution | Generate minimal stubs and create follow-up tasks for replacements. |
| Compatibility Mode | Keep .NET Framework reference with suppressed NU1701; may cause runtime failures. |

### Unsupported API Handling
Assessment shows binary and source incompatible APIs (Api.0001, Api.0002); fix inline is the default for most upgrades.

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve every API change in the same task; no stubs or deferred work. |
| Defer Complex Changes | Apply simple replacements inline; stub complex ones and create resolution subtasks. |

### Windows Native APIs
System.Messaging (MSMQ) usage detected — Windows-specific API requiring migration guidance.

| Value | Description |
|-------|-------------|
| **Windows Compatibility Pack** (selected) | Add Microsoft.Windows.Compatibility; enables Windows APIs including MSMQ in .NET 10. App remains Windows-only until APIs are replaced. |
| No Compatibility Pack | MSMQ APIs surface as build errors immediately; must be replaced with cross-platform alternatives (e.g., Azure Service Bus). |

### System.Web Adapters
System.Web references detected in an ASP.NET MVC project; side-by-side migration selected — adapters enable incremental migration.

| Value | Description |
|-------|-------------|
| **Use System.Web Adapters** (selected) | Add Microsoft.AspNetCore.SystemWebAdapters; provides HttpContext.Current shims for incremental migration. Requires cleanup pass after migration completes. |
| Direct Migration to ASP.NET Core APIs | No adapter shims; replace all System.Web usage with native ASP.NET Core equivalents upfront. More work, cleaner result. |

## Modernization

### Configuration Migration
web.config present with 5 appSettings keys, 1 connection string, no custom sections, no encryption — standard configuration only.

| Value | Description |
|-------|-------------|
| **Auto-migrate to .NET Core Configuration** (selected) | Automatically convert web.config to appsettings.json and migrate code to IConfiguration. |
| Manual Migration with Mapping Document | Generate detailed mapping before migration; more control for complex configs. |

### Assembly Binding Redirects
22 binding redirects found in web.config — volume (> 10) warrants review before bulk removal.

| Value | Description |
|-------|-------------|
| **Document and Review Before Removing** (selected) | Generate report of all redirects and their purposes; review for real underlying conflicts before removal. |
| Remove Binding Redirects | Remove all redirects; .NET Core handles assembly resolution differently and does not need them. |

### Nullable Reference Types
Target is net10.0 (supports nullable); project not yet enabled; assessment Difficulty=High — enabling during an already complex migration would add noise.

| Value | Description |
|-------|-------------|
| **Leave Disabled** (selected) | Do not enable nullable; maintain existing null handling; enable separately after migration as a distinct effort. |
| Enable Nullable Reference Types | Add `<Nullable>enable</Nullable>` to project files; compile-time null safety; may require code updates. |
