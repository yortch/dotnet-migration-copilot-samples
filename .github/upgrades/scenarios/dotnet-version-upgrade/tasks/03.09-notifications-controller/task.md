# 03.09-notifications-controller: Migrate NotificationsController and its views

# 03.09-notifications-controller

## Objective
Move NotificationsController and its views to ASP.NET Core MVC using the replacement notification service.

## Scope
- Controllers/NotificationsController.cs
- Views/Notifications/*
- Notification service consumers

## Steps
1. Update controller actions and service usage for ASP.NET Core.
2. Ensure notification listing and read/update flows work with the replacement service.
3. Build the project and fix all warnings in touched files.
