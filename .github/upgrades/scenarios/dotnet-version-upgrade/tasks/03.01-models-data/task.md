# 03.01-models-data: Migrate models, PaginatedList, data layer (SchoolContext, DbInitializer)

## Objective
Migrate all model classes, PaginatedList, and the data layer (SchoolContext, DbInitializer) from the legacy project to ContosoUniversity.Web. These classes are clean C# with no System.Web dependencies.

## Scope
- Models: Course, CourseAssignment, Department, Enrollment, ErrorViewModel, Instructor, Notification, OfficeAssignment, Person, Student
- ViewModels: AssignedCourseData, EnrollmentDateGroup, InstructorIndexData
- PaginatedList.cs
- Data: SchoolContext (full, replaces scaffold), DbInitializer, SchoolContextFactory (adapt to IConfiguration)

## Steps
1. Create Models folder structure in ContosoUniversity.Web
2. Copy and update namespaces for all model files
3. Replace scaffold SchoolContext.cs with full implementation (EF Core 10.0.10)
4. Migrate DbInitializer
5. Remove SchoolContextFactory (replace with DI-based approach in Program.cs)
6. Build and verify 0 errors

## Done when
- All model classes present in ContosoUniversity.Web with namespace ContosoUniversity.Web.*
- SchoolContext has full DbSet definitions and OnModelCreating
- dotnet build succeeds with 0 errors
