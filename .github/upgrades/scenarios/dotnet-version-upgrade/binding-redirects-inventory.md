# Binding Redirects Inventory — ContosoUniversity

**Source**: `ContosoUniversity\Web.config` `<runtime><assemblyBinding>` section  
**Status**: Not applicable in new `ContosoUniversityCore` project (.NET 10 handles assembly resolution automatically; no `app.config`/`web.config` binding redirects are used).

## All 22 Redirects from Legacy Web.config

| Assembly | Old Version Range | Redirected To | Analysis |
|----------|-------------------|---------------|----------|
| Microsoft.Web.Infrastructure | 0.0.0.0–2.0.1.0 | 2.0.1.0 | **Remove** — ASP.NET Framework infrastructure; not needed in .NET 10 |
| Antlr3.Runtime | 0.0.0.0–3.4.1.9004 | 3.4.1.9004 | **Remove** — Replaced by Antlr4 package in ContosoUniversityCore |
| Newtonsoft.Json | 0.0.0.0–13.0.0.0 | 13.0.0.0 | **Remove** — NuGet package management handles this in SDK-style projects |
| System.Web.Optimization | 1.0.0.0–1.1.0.0 | 1.1.0.0 | **Remove** — Package is incompatible with .NET 10; replaced with direct `<link>`/`<script>` tags |
| WebGrease | 0.0.0.0–1.5.2.14234 | 1.5.2.14234 | **Remove** — Dependency of Web.Optimization; not used in Core project |
| System.Web.Helpers | 1.0.0.0–3.0.0.0 | 3.0.0.0 | **Remove** — ASP.NET Framework helpers; not needed in .NET 10 |
| System.Web.WebPages | 1.0.0.0–3.0.0.0 | 3.0.0.0 | **Remove** — ASP.NET Framework WebPages; not needed in .NET 10 |
| System.Web.Mvc | 1.0.0.0–5.2.9.0 | 5.2.9.0 | **Remove** — ASP.NET MVC 5; replaced by `Microsoft.AspNetCore.Mvc` |
| System.Threading.Tasks.Extensions | 0.0.0.0–4.2.0.1 | 4.2.0.1 | **Remove** — Included in .NET 10 BCL; no redirect needed ⚠️ Binding.0006 conflict |
| Microsoft.Extensions.DependencyInjection.Abstractions | 0.0.0.0–3.1.32.0 | 3.1.32.0 | **Remove** — ⚠️ Binding.0007 version downgrade (3.1.32 → should be 10.x); resolved by removing |
| Microsoft.Extensions.DependencyInjection | 0.0.0.0–3.1.32.0 | 3.1.32.0 | **Remove** — ⚠️ Binding.0007 version downgrade; resolved by removing |
| Microsoft.EntityFrameworkCore.Abstractions | 0.0.0.0–3.1.32.0 | 3.1.32.0 | **Remove** — ⚠️ Binding.0007 version downgrade; EF Core 10.x used in Core project |
| Microsoft.Extensions.Caching.Abstractions | 0.0.0.0–3.1.32.0 | 3.1.32.0 | **Remove** — ⚠️ Binding.0007 version downgrade; resolved by removing |
| Microsoft.Extensions.Configuration.Abstractions | 0.0.0.0–3.1.32.0 | 3.1.32.0 | **Remove** — ⚠️ Binding.0007 version downgrade; resolved by removing |
| Microsoft.Extensions.Logging.Abstractions | 0.0.0.0–3.1.32.0 | 3.1.32.0 | **Remove** — ⚠️ Binding.0007 version downgrade; resolved by removing |
| Microsoft.Extensions.Options | 0.0.0.0–3.1.32.0 | 3.1.32.0 | **Remove** — ⚠️ Binding.0007 version downgrade; resolved by removing |
| Microsoft.Extensions.Primitives | 0.0.0.0–3.1.32.0 | 3.1.32.0 | **Remove** — ⚠️ Binding.0007 version downgrade; resolved by removing |
| System.ComponentModel.Annotations | 0.0.0.0–4.2.1.0 | 4.2.1.0 | **Remove** — Included in .NET 10 BCL; no redirect needed ⚠️ Binding.0006 conflict |
| System.Runtime.CompilerServices.Unsafe | 0.0.0.0–4.0.6.0 | 4.0.6.0 | **Remove** — Included in .NET 10 BCL ⚠️ Binding.0006 conflict |
| System.Memory | 0.0.0.0–4.0.1.1 | 4.0.1.1 | **Remove** — Included in .NET 10 BCL ⚠️ Binding.0006 conflict |
| Microsoft.Data.SqlClient | 0.0.0.0–2.0.20168.4 | 2.0.20168.4 | **Remove** — ⚠️ Binding.0006/0007 conflicts; upgraded to 7.0.2 in Core project (security fix) |
| netstandard | 0.0.0.0–2.0.0.0 | 2.0.0.0 | **Remove** — Not applicable in .NET 10 (netstandard is legacy) ⚠️ Binding.0006 conflict |

## Resolution

- **All 22 binding redirects**: Not needed in the new `ContosoUniversityCore` (.NET 10) project.
- The old `ContosoUniversity` Framework project retains its `Web.config` intact (not modified per side-by-side migration constraints).
- The 6 Binding.0006 conflicts and 6 Binding.0007 conflicts identified in the assessment are all resolved by the move to .NET 10 (no binding redirect mechanism in modern .NET).
- The version-downgrade redirects (Microsoft.Extensions.*, Microsoft.EntityFrameworkCore.Abstractions, Microsoft.Data.SqlClient) reflected that the packages were at 3.1.x while the assemblies expected newer versions. All are now at their correct versions (10.0.x for Extensions, 10.0.10 for EF Core, 7.0.2 for SqlClient).

## Post-Upgrade Action for User

When removing the legacy `ContosoUniversity` Framework project from the solution:
1. The `Web.config` can be deleted along with the entire old project directory.
2. No binding redirect configuration needs to be carried forward to the new project.
