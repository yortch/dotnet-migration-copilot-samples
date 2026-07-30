# 03.09-static-views: Migrate static assets, shared views, remove YARP proxy

## Objective
Migrate static files (CSS, JS), update shared views and layout, replace bundling with direct script/link tags, and remove YARP proxy from Program.cs once all routes are implemented.

## Scope
- wwwroot/: copy Content/Scripts static files
- Views/Shared/Error.cshtml
- App_Start/BundleConfig.cs -> remove; replace @Scripts.Render / @Styles.Render with direct tags in _Layout.cshtml
- Remove YARP proxy configuration from Program.cs and appsettings.json
- Remove Yarp.ReverseProxy package reference

## Done when
- Static assets available in wwwroot
- Bundling replaced with direct HTML tags
- YARP removed
- dotnet build succeeds with 0 errors
