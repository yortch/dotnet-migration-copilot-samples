
## [2026-04-09 15:29] 01-prerequisites

Captured the ContosoUniversity upgrade baseline. Documented the legacy project shape, startup/configuration entry points, and the MSMQ/bundling areas that will need replacement. Verified that no test projects exist and recorded the current build failure: `dotnet build` on the legacy solution stops with MSB4019 because `Microsoft.WebApplication.targets` is unavailable until the project is converted away from the classic web application format.

