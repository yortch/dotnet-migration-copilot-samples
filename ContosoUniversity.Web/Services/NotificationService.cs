using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ContosoUniversity.Web.Models;
using Newtonsoft.Json;

namespace ContosoUniversity.Web.Services
{
    public class NotificationService
    {
        private readonly ConcurrentQueue<Notification> _queue = new ConcurrentQueue<Notification>();
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IConfiguration configuration, ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public void SendNotification(string entityType, string entityId, EntityOperation operation, string userName = null)
        {
            SendNotification(entityType, entityId, null, operation, userName);
        }

        public void SendNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation, string userName = null)
        {
            try
            {
                var notification = new Notification
                {
                    EntityType = entityType,
                    EntityId = entityId,
                    Operation = operation.ToString(),
                    Message = GenerateMessage(entityType, entityId, entityDisplayName, operation),
                    CreatedAt = DateTime.Now,
                    CreatedBy = userName ?? "System",
                    IsRead = false
                };

                _queue.Enqueue(notification);
                _logger.LogInformation("Notification queued: {Message}", notification.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification");
            }
        }

        public Notification ReceiveNotification()
        {
            if (_queue.TryDequeue(out var notification))
            {
                return notification;
            }
            return null;
        }

        public void MarkAsRead(int notificationId)
        {
            // In a full implementation, persist notifications to the database and update read status
        }

        private string GenerateMessage(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
        {
            var displayText = !string.IsNullOrWhiteSpace(entityDisplayName)
                ? $"{entityType} '{entityDisplayName}'"
                : $"{entityType} (ID: {entityId})";

            return operation switch
            {
                EntityOperation.CREATE => $"New {displayText} has been created",
                EntityOperation.UPDATE => $"{displayText} has been updated",
                EntityOperation.DELETE => $"{displayText} has been deleted",
                _ => $"{displayText} operation: {operation}"
            };
        }

        public void Dispose()
        {
        }
    }
}

