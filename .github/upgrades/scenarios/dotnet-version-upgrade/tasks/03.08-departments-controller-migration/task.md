# 03.08-departments-controller-migration: Migrate DepartmentsController (94 API issues) and its Views/Departments folder to net10.0

## Objective
Resolve the 94 `Api.0001` issues in `Controllers/DepartmentsController.cs` and migrate its `Views/Departments/` views (including the two bundling-reference views already handled by 03.02 — do not re-touch those bundling lines here).

Runs after 03.02.

## Scope
`Controllers/DepartmentsController.cs`, `Views/Departments/*.cshtml`.

## Steps
1. Use `get_code_dependencies` on `DepartmentsController.cs` to get its full dependency tree (models: `Department`, `Instructor`; concurrency handling via `RowVersion`; views) before making changes.
2. Work through the `Index`, `Details`, `Create` (x2), `Edit` (x2, includes optimistic-concurrency `RowVersion` handling), `Delete`, `DeleteConfirmed` actions, fixing each flagged `System.Web.Mvc` API usage.
3. Build and verify 0 API issues remain for this controller/its views.

## Done when
`DepartmentsController.cs` and `Views/Departments/` build with 0 API issues.
