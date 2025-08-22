using System;
using System.Web;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            // RouteConfig removed; routing now configured in Program.cs
            // BundleConfig removed; bundling replaced with direct tags/CDN

            // Initialize database with EF Core
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
            optionsBuilder.UseSqlServer(connectionString);
            
            using (var context = new SchoolContext(optionsBuilder.Options))
            {
                DbInitializer.Initialize(context);
            }
        }
    }
}
