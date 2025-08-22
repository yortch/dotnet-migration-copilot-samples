using System;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;

namespace ContosoUniversity
{
    public class MvcApplication
    {
        protected void Application_Start()
        {
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
