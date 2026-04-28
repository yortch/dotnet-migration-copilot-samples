using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers;

public abstract class BaseController : Controller
{
    protected readonly SchoolContext Db;
    protected readonly NotificationService NotificationService;

    protected BaseController(SchoolContext db, NotificationService notificationService)
    {
        Db = db;
        NotificationService = notificationService;
    }

    protected void SendEntityNotification(string entityType, string entityId, EntityOperation operation)
    {
        SendEntityNotification(entityType, entityId, null, operation);
    }

    protected void SendEntityNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
    {
        try
        {
            NotificationService.SendNotification(entityType, entityId, entityDisplayName, operation, "System");
        }
        catch (Exception ex)
        {
            HttpContext.RequestServices
                .GetRequiredService<ILogger<BaseController>>()
                .LogError(ex, "Failed to send notification for {EntityType} {EntityId}", entityType, entityId);
        }
    }
}
