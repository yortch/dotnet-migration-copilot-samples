# 03.05-courses: Migrate CoursesController and Course views (includes file upload)

## Objective
Migrate CoursesController to ASP.NET Core, along with all Course views. This controller has file upload functionality using HttpPostedFileBase and Server.MapPath.

## Scope
- Controllers/CoursesController.cs
- Views/Courses/*.cshtml (Index, Details, Create, Edit, Delete)

## Key changes
- HttpPostedFileBase -> IFormFile
- Server.MapPath("~/Uploads/") -> IWebHostEnvironment.WebRootPath or Path.Combine(env.ContentRootPath, "Uploads")
- ActionResult -> IActionResult
- Inject IWebHostEnvironment via constructor

## Done when
- CoursesController compiles with 0 errors
- File upload uses IFormFile
- Course views migrated
- dotnet build succeeds with 0 errors
