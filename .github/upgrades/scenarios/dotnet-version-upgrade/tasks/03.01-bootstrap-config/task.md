# 03.01-bootstrap-config: Rebuild project bootstrap and configuration for ASP.NET Core

# 03.01-bootstrap-config

## Objective
Convert the project file, startup path, package graph, and configuration surface to ASP.NET Core on .NET 10.

## Scope
- ContosoUniversity.csproj
- Program.cs and app startup files
- Web.config / appsettings.json
- Data/SchoolContextFactory.cs and EF Core startup wiring

## Steps
1. Replace the interim net48 SDK-style project configuration with the final net10.0 package and project setup.
2. Introduce ASP.NET Core hosting, DI registration, EF Core setup, and database initialization.
3. Move configuration from Web.config to appsettings.json and remove obsolete startup artifacts.
4. Build the project and fix all warnings in the touched files.
