# 03.05-home-controller-migration: Migrate HomeController and its Views/Home folder to net10.0 (minimal API surface)

## Objective
Migrate `Controllers/HomeController.cs` and its `Views/Home/` views to build and run correctly on net10.0 with the System.Web Adapters bridge in place (from 03.02). Per the assessment, this controller has no significant flagged API issues of its own — this subtask verifies it end-to-end and is a good simplest-first starting point.

Runs after 03.02 (needs `BaseController` and hosting scaffolding in place).

## Scope
`Controllers/HomeController.cs`, `Views/Home/*.cshtml`, `Views/Shared/Error.cshtml` if referenced by `HomeController.Error()`.

## Steps
1. Use `get_code_dependencies` on `HomeController.cs` to confirm its full dependency tree (services, models, views).
2. Build and fix any residual `System.Web.Mvc` API issues on this controller/its views (expected to be minimal/none per assessment).
3. Verify `Index`, `About`, `Contact`, `Error`, `Unauthorized` actions compile and return views correctly under the adapters bridge.

## Done when
`HomeController.cs` and `Views/Home/` build with 0 API issues and no remaining assessment findings for this controller.
