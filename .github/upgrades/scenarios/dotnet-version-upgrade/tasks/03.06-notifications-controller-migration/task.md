# 03.06-notifications-controller-migration: Migrate NotificationsController and its Views/Notifications folder to net10.0

## Objective
Migrate `Controllers/NotificationsController.cs` and its `Views/Notifications/` views to net10.0. This controller calls `notificationService.ReceiveNotification()` and `.MarkAsRead()` directly, so it depends on subtask 03.04 (MSMQ replacement) having landed first with unchanged public signatures.

Runs after 03.02 and 03.04.

## Scope
`Controllers/NotificationsController.cs`, `Views/Notifications/*.cshtml`.

## Steps
1. Confirm `Services/NotificationService.cs` (from 03.04) exposes `ReceiveNotification()`, `MarkAsRead(int)` with the same signatures this controller already calls — no changes needed if 03.04 preserved them.
2. Use `get_code_dependencies` to confirm the full dependency tree.
3. Build and fix any residual `System.Web.Mvc`/`JsonResult` API issues (expected to be minimal per assessment — this controller mostly uses JSON actions, not flagged directly).
4. Verify `GetNotifications`, `MarkAsRead`, `Index` actions compile and function under the adapters bridge.

## Done when
`NotificationsController.cs` and `Views/Notifications/` build with 0 API issues, and correctly call the replaced `NotificationService` API.
