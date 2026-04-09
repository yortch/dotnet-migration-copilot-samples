# 03.04-base-controller: Migrate shared controller infrastructure

# 03.04-base-controller

## Objective
Update the shared controller base and helper behaviors for ASP.NET Core MVC.

## Scope
- Controllers/BaseController.cs
- Shared upload, notification, and helper methods used across derived controllers

## Steps
1. Replace legacy MVC base types and request/file abstractions with ASP.NET Core equivalents.
2. Keep derived controllers compatible with the updated base behavior.
3. Build the project and fix all warnings in touched files.
