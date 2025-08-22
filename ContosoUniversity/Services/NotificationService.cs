using System;
using System.Threading.Tasks;
using System.Configuration; // kept for backward compat if needed
using ContosoUniversity.Models;
using Newtonsoft.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ContosoUniversity.Services
{
    public class NotificationService : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly bool _enabled;

        public NotificationService()
        {
            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            _connectionString = config["Notification:ServiceBusConnectionString"];
            _queueName = config["Notification:QueueName"] ?? "contosouniversity-notifications";

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _enabled = false;
                System.Diagnostics.Debug.WriteLine("NotificationService disabled: ServiceBus connection string not configured.");
                return;
            }

            _enabled = true;
            _client = new ServiceBusClient(_connectionString);
            _sender = _client.CreateSender(_queueName);
        }

        public void SendNotification(string entityType, string entityId, EntityOperation operation, string userName = null)
        {
            SendNotification(entityType, entityId, null, operation, userName);
        }

        public void SendNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation, string userName = null)
        {
            try
            {
                if (!_enabled)
                {
                    return;
                }

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

                var jsonMessage = JsonConvert.SerializeObject(notification);
                var message = new ServiceBusMessage(jsonMessage)
                {
                    Subject = $"{entityType} {operation}"
                };

                // send synchronously-compatible
                _sender.SendMessageAsync(message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Log error but don't break the main operation
                System.Diagnostics.Debug.WriteLine($"Failed to send notification: {ex.Message}");
            }
        }

        public Notification ReceiveNotification()
        {
            try
            {
                if (!_enabled)
                {
                    return null;
                }

                // Use a receiver scoped for this call to avoid keeping open long-lived receivers
                var receiver = _client.CreateReceiver(_queueName);
                var message = receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();
                if (message == null)
                {
                    return null;
                }

                var jsonContent = message.Body.ToString();
                var notification = JsonConvert.DeserializeObject<Notification>(jsonContent);

                // Complete message to remove from queue
                receiver.CompleteMessageAsync(message).GetAwaiter().GetResult();
                receiver.DisposeAsync().AsTask().GetAwaiter().GetResult();
                return notification;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to receive notification: {ex.Message}");
                return null;
            }
        }

        public void MarkAsRead(int notificationId)
        {
            // In a real implementation, you might want to store notifications in database as well
            // for persistence and tracking read status
        }

        private string GenerateMessage(string entityType, string entityId, string entityDisplayName, EntityOperation operation)
        {
            var displayText = !string.IsNullOrWhiteSpace(entityDisplayName) 
                ? $"{entityType} '{entityDisplayName}'" 
                : $"{entityType} (ID: {entityId})";

            switch (operation)
            {
                case EntityOperation.CREATE:
                    return $"New {displayText} has been created";
                case EntityOperation.UPDATE:
                    return $"{displayText} has been updated";
                case EntityOperation.DELETE:
                    return $"{displayText} has been deleted";
                default:
                    return $"{displayText} operation: {operation}";
            }
        }

        public void Dispose()
        {
            try
            {
                _sender?.DisposeAsync().AsTask().GetAwaiter().GetResult();
                _client?.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch { }
        }
    }
}
