# 03.02-services: Migrate NotificationService (MSMQ via Windows.Compatibility)

## Objective
Migrate NotificationService from the legacy project to ContosoUniversity.Web. The service uses System.Messaging (MSMQ) which is available via Microsoft.Windows.Compatibility.

## Scope
- Services/NotificationService.cs
- Replace ConfigurationManager.AppSettings with IConfiguration
- Replace System.Messaging references (available via Microsoft.Windows.Compatibility)

## Steps
1. Create Services folder in ContosoUniversity.Web
2. Migrate NotificationService with IConfiguration dependency injection
3. Register INotificationService in Program.cs DI
4. Build and verify 0 errors

## Done when
- NotificationService present in ContosoUniversity.Web.Services
- Uses IConfiguration instead of ConfigurationManager
- Registered in Program.cs DI
- dotnet build succeeds with 0 errors
