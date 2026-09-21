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
    using var context = SchoolContextFactory.Create();
    DbInitializer.Initialize(context);
}
