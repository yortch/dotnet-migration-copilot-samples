# 02-sdk-style-conversion Progress Details

- Used the SDK-style conversion tool on `ContosoUniversity.csproj`.
- Verified the project file is now SDK-style and that package declarations were moved into `PackageReference` items.
- Removed the obsolete custom SQL client copy target and deleted `packages.config` in preparation for the rewrite.
- Attempted a direct `dotnet build` of the converted `net48` project and hit restore-time blockers: the existing vulnerable `Microsoft.Data.SqlClient` package and multiple transitive package downgrade conflicts.
- Recorded the blockers in `task.md` so the package resolution work can continue in the .NET 10 rewrite task.
