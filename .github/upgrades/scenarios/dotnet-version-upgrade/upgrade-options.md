# Upgrade Options — ContosoUniversity

Assessment: 1 project on .NET Framework 4.8, ASP.NET MVC 5, EF Core 3.1, 147 issues, 45 packages

## Strategy

### Upgrade Strategy
Single .NET Framework project — All-at-Once is the only applicable strategy.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade the single project in one atomic pass. No dependency graph to manage. |

## Project Structure

### Project Approach
Single ASP.NET MVC 5 web project with 7 controllers and moderate complexity.

| Value | Description |
|-------|-------------|
| **In-place rewrite** (selected) | Replace the Framework web project entirely in one pass. Suitable for small-to-medium projects. |
| Side-by-side | Create new ASP.NET Core project alongside old one with incremental migration. |

## Compatibility

### Unsupported Packages
1 incompatible package (Microsoft.AspNet.Web.Optimization) with no compatible version for net10.0.

| Value | Description |
|-------|-------------|
| **Resolve Inline** (selected) | Research and resolve incompatible package within the same task. |
| Defer Resolution | Generate stubs and create follow-up tasks for replacements. |

### Unsupported API Handling
63 binary-incompatible and 29 source-incompatible API changes detected.

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve every API change in the same task, including complex ones. |
| Defer Complex Changes | Apply simple replacements inline, defer complex changes to stubs. |

### System.Web Adapters
ASP.NET MVC 5 project with System.Web references throughout.

| Value | Description |
|-------|-------------|
| **Direct Migration to ASP.NET Core APIs** (selected) | Replace all System.Web usage with native ASP.NET Core equivalents. Cleaner result. |
| Use System.Web Adapters | Add compatibility shims for incremental migration. |

## Modernization

### Configuration Migration
Standard Web.config with appSettings and connectionStrings.

| Value | Description |
|-------|-------------|
| **Auto-migrate to .NET Core Configuration** (selected) | Convert Web.config to appsettings.json and IConfiguration automatically. |
| Manual Migration with Mapping Document | Generate detailed mapping before migration. |

### Assembly Binding Redirects
Auto-generated binding redirects in Web.config.

| Value | Description |
|-------|-------------|
| **Remove Binding Redirects** (selected) | Remove all redirects. .NET Core handles assembly resolution differently. |
| Document and Review Before Removing | Generate report of all redirects before removal. |

### Nullable Reference Types
Target is net10.0 with 147 issues and complex migration.

| Value | Description |
|-------|-------------|
| **Leave Disabled** (selected) | Do not enable nullable. Enable separately after migration. |
| Enable Nullable Reference Types | Add nullable enable to project files. |
