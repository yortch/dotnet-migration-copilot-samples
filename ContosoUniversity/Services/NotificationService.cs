using ContosoUniversity.Models;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ContosoUniversity.Services
{
    public class NotificationService
    {
        private readonly ConcurrentQueue<Notification> queue = new();
        private readonly ConcurrentDictionary<int, bool> readNotifications = new();
        private int nextId = 1;

        public void SendNotification(string entityType, string entityId, EntityOperation operation, string userName = null)
        {
            SendNotification(entityType, entityId, null, operation, userName);
        }

        public void SendNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation, string userName = null)
        {
            var notification = new Notification
            {
                Id = Interlocked.Increment(ref nextId),
                EntityType = entityType,
                EntityId = entityId,
                Operation = operation.ToString(),
                Message = GenerateMessage(entityType, entityId, entityDisplayName, operation),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userName ?? "System",
                IsRead = false
            };

            queue.Enqueue(notification);
        }

        public Notification ReceiveNotification()
        {
            while (queue.TryDequeue(out var notification))
            {
                if (!readNotifications.ContainsKey(notification.Id))
                {
                    return notification;
                }
            }

            return null;
        }

        public void MarkAsRead(int notificationId)
        {
            readNotifications[notificationId] = true;
        }

        private static string GenerateMessage(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
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
    }
}
