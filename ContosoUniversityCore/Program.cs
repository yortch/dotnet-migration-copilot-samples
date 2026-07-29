using ContosoUniversityCore.Data;
using ContosoUniversityCore.Services;
using Microsoft.AspNetCore.SystemWebAdapters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// EF Core - SQL Server database context (shared ContosoUniversityNoAuthEFCore database)
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Application services ported from the ASP.NET Framework project
builder.Services.AddScoped<NotificationService>();

// System.Web compatibility shim to ease incremental migration from ASP.NET Framework
builder.Services.AddSystemWebAdapters();

// YARP reverse proxy: forwards any request not handled by this app to the legacy
// ASP.NET Framework application during the side-by-side migration.
builder.Services.AddReverseProxy()
    .LoadFromMemory(new[]
    {
        new Yarp.ReverseProxy.Configuration.RouteConfig
        {
            RouteId = "legacy-fallback",
            ClusterId = "legacy-cluster",
            Match = new Yarp.ReverseProxy.Configuration.RouteMatch
            {
                Path = "/{**catch-all}"
            },
            Order = int.MaxValue
        }
    }, new[]
    {
        new Yarp.ReverseProxy.Configuration.ClusterConfig
        {
            ClusterId = "legacy-cluster",
            Destinations = new Dictionary<string, Yarp.ReverseProxy.Configuration.DestinationConfig>
            {
                // Placeholder upstream URL for the legacy ASP.NET Framework app.
                // Update this once the legacy app's actual host/port is known.
                ["legacy-app"] = new Yarp.ReverseProxy.Configuration.DestinationConfig
                {
                    Address = "http://localhost:5000"
                }
            }
        }
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseSystemWebAdapters();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Any request not matched by an MVC route falls through to the legacy Framework app.
app.MapReverseProxy();

app.Run();
