#nullable disable

using System;
using ContosoUniversityCore.Data;
using ContosoUniversityCore.Models;
using ContosoUniversityCore.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversityCore.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly SchoolContext db;
        protected readonly NotificationService notificationService;
        protected readonly IWebHostEnvironment WebHostEnvironment;

        protected BaseController(SchoolContext db, NotificationService notificationService, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.notificationService = notificationService;
            WebHostEnvironment = webHostEnvironment;
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
                notificationService.SendNotification(entityType, entityId, entityDisplayName, operation, userName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send notification: {ex.Message}");
            }
        }
    }
}
