// STUB: Scaffold placeholder — to be replaced with full SchoolContext during task 03-migrate-contoso
// Full implementation migrated from ContosoUniversity/Data/SchoolContext.cs
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Web.Data;

/// <summary>
/// Scaffold placeholder for SchoolContext. 
/// Full EF Core DbContext will be migrated from the legacy project in task 03-migrate-contoso.
/// </summary>
public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
    }
}
