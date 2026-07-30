# Migration Progress

**Progress**: 2/12 tasks complete <progress value="17" max="100"></progress> 17%
**Status**: In Progress - Task 03-migrate-contoso

## Tasks

- ✅ 01-prerequisites: Verify SDK and toolchain readiness ([Content](tasks/01-prerequisites/task.md), [Progress](tasks/01-prerequisites/progress-details.md))
- ✅ 02-scaffold-contoso: Scaffold new ASP.NET Core project alongside legacy project ([Content](tasks/02-scaffold-contoso/task.md), [Progress](tasks/02-scaffold-contoso/progress-details.md))
- 🔄 03-migrate-contoso: Migrate all web assets from legacy to ASP.NET Core project ([Content](tasks/03-migrate-contoso/task.md))
  - 🔄 03.01-models-data: Migrate models, PaginatedList, data layer (SchoolContext, DbInitializer) ([Content](tasks/03.01-models-data/task.md))
  - 🔲 03.02-services: Migrate NotificationService (MSMQ via Windows.Compatibility)
  - 🔲 03.03-base-home: Migrate BaseController, HomeController, and Home views
  - 🔲 03.04-students: Migrate StudentsController and Student views
  - 🔲 03.05-courses: Migrate CoursesController and Course views (includes file upload)
  - 🔲 03.06-instructors: Migrate InstructorsController and Instructor views
  - 🔲 03.07-departments: Migrate DepartmentsController and Department views
  - 🔲 03.08-notifications: Migrate NotificationsController and Notification views
  - 🔲 03.09-static-views: Migrate static assets, shared views, remove YARP proxy
- 🔲 04-final-validation: Final build validation and post-upgrade documentation

**Legend**: ✅ Complete | 🔄 In Progress | 🔲 Pending | ⚠️ Blocked | ❌ Failed
