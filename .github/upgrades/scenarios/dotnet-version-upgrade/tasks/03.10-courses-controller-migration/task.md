# 03.10-courses-controller-migration: Migrate CoursesController (146 API issues, largest — includes file upload) and its Views/Courses folder to net10.0

## Objective
Resolve the 146 issues (128 `Api.0001` + 18 `Api.0002`) in `Controllers/CoursesController.cs` — the largest and last controller subtask, since it includes `HttpPostedFileBase` file-upload handling (`teachingMaterialImage`) which is itself System.Web-specific and needs the adapters bridge (or an `IFormFile`-based rework) fully proven out by the simpler controllers first. Migrate its `Views/Courses/` views (including the two bundling-reference views already handled by 03.02 — do not re-touch those bundling lines here).

Runs after 03.02, and after 03.07/03.08/03.09 have proven out the adapters-bridge pattern for simpler CRUD controllers.

## Scope
`Controllers/CoursesController.cs`, `Views/Courses/*.cshtml`.

## Steps
1. Use `get_code_dependencies` on `CoursesController.cs` to get its full dependency tree (models: `Course`, `Department`; file upload for `TeachingMaterialImagePath`; see [TEACHING_MATERIAL_UPLOAD.md](../../../../../TEACHING_MATERIAL_UPLOAD.md) for existing feature context) before making changes.
2. Work through the `Index`, `Details`, `Create` (x2, incl. `HttpPostedFileBase teachingMaterialImage` upload handling), `Edit` (x2, same upload handling), `Delete`, `DeleteConfirmed` actions, fixing each flagged `System.Web.Mvc` API usage. Pay particular attention to the file-upload code path — confirm whether the adapters bridge (`HttpPostedFileBase`) covers it or whether it needs converting to `IFormFile`.
3. Build and verify 0 API issues remain for this controller/its views.

## Done when
`CoursesController.cs` and `Views/Courses/` build with 0 API issues, and the teaching-material image upload continues to function.
