using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Services;

public class NotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly SchoolContext _dbContext;

    public NotificationService(SchoolContext dbContext, ILogger<NotificationService> logger)
    {
        _dbContext = dbContext;
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
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userName ?? "System",
                IsRead = false
            };

            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save notification for {EntityType} {EntityId}", entityType, entityId);
        }
    }

    public IReadOnlyList<Notification> GetUnreadNotifications(int limit = 10)
    {
        var notifications = _dbContext.Notifications
            .Where(notification => !notification.IsRead)
            .OrderByDescending(notification => notification.CreatedAt)
            .Take(limit)
            .ToList();

        if (notifications.Count == 0)
        {
            return notifications;
        }

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }

        _dbContext.SaveChanges();
        return notifications;
    }

    public void MarkAsRead(int notificationId)
    {
        var notification = _dbContext.Notifications.SingleOrDefault(item => item.Id == notificationId);
        if (notification is null || notification.IsRead)
        {
            return;
        }

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        _dbContext.SaveChanges();
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
