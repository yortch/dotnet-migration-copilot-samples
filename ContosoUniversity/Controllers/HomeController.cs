using System.Diagnostics;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers;

public class HomeController : BaseController
{
    public HomeController(SchoolContext db, NotificationService notificationService)
        : base(db, notificationService)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        var data = Db.Students
            .AsNoTracking()
            .GroupBy(student => student.EnrollmentDate)
            .Select(dateGroup => new EnrollmentDateGroup
            {
                EnrollmentDate = dateGroup.Key,
                StudentCount = dateGroup.Count()
            })
            .OrderBy(item => item.EnrollmentDate)
            .ToList();

        return View(data);
    }

    public IActionResult Contact()
    {
        ViewData["Message"] = "Your contact page.";
        return View();
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [ActionName("Unauthorized")]
    public IActionResult UnauthorizedPage()
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}
