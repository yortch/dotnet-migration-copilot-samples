# 02-scaffold-contoso: Scaffold new ASP.NET Core project alongside legacy project

## Scope Inventory

- **New project**: `ContosoUniversity.Web` (net10.0, SDK-style)
- **Old project**: `ContosoUniversity` (net48, stays unchanged)
- **Distinct concerns**: project creation, DI/routing bootstrap, YARP config, appsettings, stub controller

## Research Findings

### Packages (confirmed versions for net10.0)
- `Yarp.ReverseProxy` 2.3.0
- `Microsoft.AspNetCore.SystemWebAdapters` 2.3.0
- `Microsoft.Windows.Compatibility` 10.0.10
- `Microsoft.EntityFrameworkCore.SqlServer` 10.0.10
- `Microsoft.Data.SqlClient` 7.0.2 (security fix)
- `Newtonsoft.Json` 13.0.4

### Existing Project Config to Migrate
- Connection string: `(LocalDb)\MSSQLLocalDB;Initial Catalog=ContosoUniversityNoAuthEFCore`
- Notification queue path: `.\Private$\ContosoUniversityNotifications`
- Old project URL: `https://localhost:44300/` (from Web.config IIS Express settings)

## Done When

- [ ] `ContosoUniversity.Web` project created and builds with 0 errors
- [ ] Stub HomeController returns HTTP 200
- [ ] YARP configured to forward all routes to old project (`https://localhost:44300/`)
- [ ] appsettings.json has connection string and notification queue path
- [ ] Old project still builds unchanged
- [ ] Both projects added to solution
