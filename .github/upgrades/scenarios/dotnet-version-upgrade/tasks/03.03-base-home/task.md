# 03.03-base-home: Migrate BaseController, HomeController, and Home views

## Objective
Migrate BaseController and HomeController to ASP.NET Core patterns, along with the Home views.

## Scope
- Controllers/BaseController.cs
- Controllers/HomeController.cs
- Views/Home/*.cshtml
- Views/Shared/_Layout.cshtml
- Views/_ViewStart.cshtml

## Steps
1. Rewrite BaseController: System.Web.Mvc.Controller -> Microsoft.AspNetCore.Mvc.Controller, inject SchoolContext and NotificationService via constructor DI
2. Rewrite HomeController: ActionResult -> IActionResult, remove System.Web.Mvc using, update About action using SchoolContext
3. Migrate views: update @using, remove System.Web.Mvc references
4. Build and verify 0 errors

## Done when
- BaseController uses constructor DI for SchoolContext and NotificationService
- HomeController uses IActionResult
- Home views migrate successfully
- dotnet build succeeds with 0 errors
