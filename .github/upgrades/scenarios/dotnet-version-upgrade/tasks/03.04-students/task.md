# 03.04-students: Migrate StudentsController and Student views

## Objective
Migrate StudentsController to ASP.NET Core, along with all Student views.

## Scope
- Controllers/StudentsController.cs
- Views/Students/*.cshtml (Index, Details, Create, Edit, Delete)

## Key changes
- ActionResult -> IActionResult
- [Bind(Include=...)] -> [Bind] with named properties or remove
- HttpStatusCodeResult -> StatusCode(400)
- HttpNotFound() -> NotFound()
- TryUpdateModel -> TryUpdateModelAsync or manual binding
- Use injected SchoolContext from base

## Done when
- StudentsController compiles with 0 errors
- All Student views migrated
- dotnet build succeeds with 0 errors
