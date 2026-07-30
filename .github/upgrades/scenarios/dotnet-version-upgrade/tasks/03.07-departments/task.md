# 03.07-departments: Migrate DepartmentsController and Department views

## Objective
Migrate DepartmentsController to ASP.NET Core, along with all Department views. This controller handles concurrency (DbUpdateConcurrencyException).

## Scope
- Controllers/DepartmentsController.cs
- Views/Departments/*.cshtml (Index, Details, Create, Edit, Delete)

## Key changes
- ActionResult -> IActionResult
- HttpStatusCodeResult -> StatusCode(400)
- HttpNotFound() -> NotFound()
- Concurrency handling logic is EF Core — preserved as-is

## Done when
- DepartmentsController compiles with 0 errors
- Department views migrated
- dotnet build succeeds with 0 errors
