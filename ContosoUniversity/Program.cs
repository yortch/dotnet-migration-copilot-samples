using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSystemWebAdapters();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSystemWebAdapters();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

InitializeDatabase();

app.Run();

void InitializeDatabase()
{
    // TODO(03.03): ConfigurationManager-based connection string read predates the Web.config -> appsettings.json migration; leave for that subtask.
    var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
    optionsBuilder.UseSqlServer(connectionString);

    using var context = new SchoolContext(optionsBuilder.Options);
    DbInitializer.Initialize(context);
}
