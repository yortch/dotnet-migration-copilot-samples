# 03.08-notifications: Migrate NotificationsController and Notification views

## Objective
Migrate NotificationsController to ASP.NET Core, along with the Notifications view. This controller returns JSON.

## Scope
- Controllers/NotificationsController.cs
- Views/Notifications/Index.cshtml

## Key changes
- JsonResult with JsonRequestBehavior.AllowGet -> return Json() (AllowGet not needed in Core)
- ActionResult -> IActionResult

## Done when
- NotificationsController compiles with 0 errors
- Notifications view migrated
- dotnet build succeeds with 0 errors
