# .NET 9.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 9.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9.0 upgrade.
3. Upgrade ContosoUniversity\ContosoUniversity.csproj
4. Run unit tests to validate upgrade in the projects listed below:

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                         |
|:------------------------------------|:---------------:|:-----------:|:------------------------------------|
| Antlr                               | 3.4.1.9004      | 4.6.6       | Recommended for .NET 9.0            |
| Microsoft.Bcl.AsyncInterfaces       | 1.1.1           | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Bcl.HashCode              | 1.1.1           | 6.0.0       | Recommended for .NET 9.0            |
| Microsoft.Data.SqlClient            | 2.1.4           | 6.1.1       | Security vulnerability              |
| Microsoft.EntityFrameworkCore       | 3.1.32          | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.EntityFrameworkCore.Abstractions | 3.1.32      | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.EntityFrameworkCore.Analyzers | 3.1.32       | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.EntityFrameworkCore.Relational | 3.1.32       | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.EntityFrameworkCore.SqlServer | 3.1.32       | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.EntityFrameworkCore.Tools | 3.1.32          | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.Caching.Abstractions | 3.1.32      | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.Caching.Memory | 3.1.32          | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.Configuration  | 3.1.32          | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.Configuration.Abstractions | 3.1.32 | 9.0.8  | Recommended for .NET 9.0            |
| Microsoft.Extensions.Configuration.Binder | 3.1.32      | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.DependencyInjection | 3.1.32      | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.DependencyInjection.Abstractions | 3.1.32 | 9.0.8 | Recommended for .NET 9.0            |
| Microsoft.Extensions.Logging        | 3.1.32          | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.Logging.Abstractions | 3.1.32      | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.Options        | 3.1.32          | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Extensions.Primitives     | 3.1.32          | 9.0.8       | Recommended for .NET 9.0            |
| Microsoft.Identity.Client           | 4.21.1          | 4.76.0      | Deprecated, update recommended      |
| Microsoft.AspNet.Web.Optimization   | 1.1.3           | -           | Incompatible; remove                |
| NETStandard.Library                 | 2.0.3           | -           | Included with framework reference   |
| System.Buffers                      | 4.5.1           | -           | Included with framework reference   |
| System.Collections.Immutable        | 1.7.1           | 9.0.8       | Recommended for .NET 9.0            |
| System.ComponentModel.Annotations   | 4.7.0           | -           | Included with framework reference   |
| System.Diagnostics.DiagnosticSource | 4.7.1           | 9.0.8       | Recommended for .NET 9.0            |
| System.Memory                       | 4.5.4           | -           | Included with framework reference   |
| System.Numerics.Vectors             | 4.5.0           | -           | Included with framework reference   |
| System.Runtime.CompilerServices.Unsafe | 4.5.3        | 6.1.2       | Recommended for .NET 9.0            |
| System.Threading.Tasks.Extensions   | 4.5.4           | -           | Included with framework reference   |
| Microsoft.AspNet.Mvc                | 5.2.9           | -           | Included with new framework         |
| Microsoft.AspNet.Razor              | 3.2.9           | -           | Included with new framework         |
| Microsoft.AspNet.WebPages           | 3.2.9           | -           | Included with new framework         |
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | 2.0.1 | - | Included with new framework         |
| Microsoft.Web.Infrastructure        | 2.0.1           | -           | Included with new framework         |

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### ContosoUniversity\ContosoUniversity.csproj modifications

Project properties changes:
  - Target framework should be changed from `.NETFramework,Version=v4.8` to `net9.0`.
  - Convert project file to SDK-style.

NuGet packages changes:
  - Remove Microsoft.AspNet.Web.Optimization (incompatible)
  - Update EF Core packages to 9.0.8
  - Update Microsoft.Data.SqlClient to 6.1.1 (security)
  - Update Microsoft.Identity.Client to 4.76.0 (deprecated)
  - Update Microsoft.Extensions.* packages to 9.0.8
  - Remove packages now provided by the framework

Feature upgrades:
  - Replace System.Web.Optimization with static script/style tags
  - Migrate RouteCollection to endpoint routing
  - Migrate System.Messaging MSMQ usage to .NET option (consider Azure Service Bus)
  - Move Global.asax initialization to Program.cs / Startup pattern

Other changes:
  - Verify Web.config transformations and move to appsettings.json as applicable
