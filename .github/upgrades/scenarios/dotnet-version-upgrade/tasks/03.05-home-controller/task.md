# 03.05-home-controller: Migrate HomeController and its views

# 03.05-home-controller

## Objective
Move HomeController and related views to ASP.NET Core MVC.

## Scope
- Controllers/HomeController.cs
- Views/Home/*
- Any shared models or helpers used only by HomeController

## Steps
1. Update the controller base type, action signatures, and result helpers.
2. Fix any view/runtime assumptions that depend on System.Web.
3. Build the project and fix all warnings in touched files.
