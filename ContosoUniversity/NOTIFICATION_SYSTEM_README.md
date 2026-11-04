# Notification System

The application provides polling-based notifications for create, update, and delete operations on students, courses, instructors, and departments.

`NotificationService` uses a process-local concurrent queue. The service is registered as a singleton so requests within one running application instance share notifications. Messages are consumed by the polling endpoint and are lost when the process restarts. This is a development implementation, not a durable or multi-instance message broker.

## Components

- `NotificationService`: queues and returns notification records
- `BaseController`: sends notifications after successful entity updates
- `NotificationsController`: endpoint used by the browser polling script
- `Scripts/notifications.js` and `Content/notifications.css`: polling and display UI

The application does not currently configure authentication or restrict the notification page to administrator accounts.

## Test notifications

1. Run the application using the instructions in `README.md`.
2. Open the Notifications page.
3. Create, edit, or delete an entity.
4. Observe the notification in the browser.
