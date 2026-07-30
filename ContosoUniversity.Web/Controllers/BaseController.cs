using System;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Web.Services;
using ContosoUniversity.Web.Models;
using ContosoUniversity.Web.Data;

namespace ContosoUniversity.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly SchoolContext _db;
        protected readonly NotificationService _notificationService;

        protected BaseController(SchoolContext db, NotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }

        protected void SendEntityNotification(string entityType, string entityId, EntityOperation operation)
        {
            SendEntityNotification(entityType, entityId, null, operation);
        }

        protected void SendEntityNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
        {
            try
            {
                var userName = "System";
                _notificationService.SendNotification(entityType, entityId, entityDisplayName, operation, userName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send notification: {ex.Message}");
            }
        }
    }
}
