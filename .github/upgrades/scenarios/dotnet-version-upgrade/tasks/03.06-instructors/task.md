# 03.06-instructors: Migrate InstructorsController and Instructor views

## Objective
Migrate InstructorsController to ASP.NET Core, along with all Instructor views. This controller uses TryUpdateModel.

## Scope
- Controllers/InstructorsController.cs
- Views/Instructors/*.cshtml (Index, Details, Create, Edit, Delete)

## Key changes
- ActionResult -> IActionResult
- TryUpdateModel -> TryUpdateModelAsync
- HttpStatusCodeResult -> StatusCode(400)
- HttpNotFound() -> NotFound()
- Instructor views: update HTML helpers and model references

## Done when
- InstructorsController compiles with 0 errors
- Instructor views migrated
- dotnet build succeeds with 0 errors
