# Progress Details: 03.05-courses

## Status: Completed

## Changes Made

### Controllers/CoursesController.cs
- HttpPostedFileBase → IFormFile
- Server.MapPath() → IWebHostEnvironment.WebRootPath (injected via constructor)
- ContentLength → IFormFile.Length
- Extends BaseController (inherits _db + _notificationService)

### Views/Courses/ (5 views)
- Index.cshtml — course listing with department name
- Create.cshtml — create course form
- Details.cshtml — course detail
- Edit.cshtml — edit course (dropdown for Department)
- Delete.cshtml — confirmation

## Build Result
0 errors, 0 warnings
