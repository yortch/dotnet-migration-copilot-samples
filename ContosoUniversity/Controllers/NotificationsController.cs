using ContosoUniversity.Data;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers;

public class NotificationsController : BaseController
{
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(SchoolContext db, NotificationService notificationService, ILogger<NotificationsController> logger)
        : base(db, notificationService)
    {
        _logger = logger;
    }

    [HttpGet]
    public JsonResult GetNotifications()
    {
        try
        {
            var notifications = NotificationService.GetUnreadNotifications();
            return Json(new
            {
                success = true,
                notifications,
                count = notifications.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications");
            return Json(new { success = false, message = "Error retrieving notifications" });
        }
    }

    public IActionResult Index()
    {
        return View();
    }
}
