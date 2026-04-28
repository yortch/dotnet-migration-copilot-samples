using System.Diagnostics;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers;

public class StudentsController : BaseController
{
    public StudentsController(SchoolContext db, NotificationService notificationService)
        : base(db, notificationService)
    {
    }

    public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : string.Empty;
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

        if (searchString is not null)
        {
            page = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        ViewData["CurrentFilter"] = searchString;

        var students = Db.Students.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            students = students.Where(student =>
                student.LastName.Contains(searchString) ||
                student.FirstMidName.Contains(searchString));
        }

        students = sortOrder switch
        {
            "name_desc" => students.OrderByDescending(student => student.LastName),
            "Date" => students.OrderBy(student => student.EnrollmentDate),
            "date_desc" => students.OrderByDescending(student => student.EnrollmentDate),
            _ => students.OrderBy(student => student.LastName)
        };

        const int pageSize = 10;
        var pageNumber = page ?? 1;
        return View(PaginatedList<Student>.Create(students, pageNumber, pageSize));
    }

    public IActionResult Details(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var student = Db.Students
            .AsNoTracking()
            .Include(item => item.Enrollments)
                .ThenInclude(enrollment => enrollment.Course)
            .SingleOrDefault(item => item.ID == id.Value);

        return student is null ? NotFound() : View(student);
    }

    public IActionResult Create()
    {
        var student = new Student
        {
            EnrollmentDate = DateTime.Today
        };

        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("LastName,FirstMidName,EnrollmentDate")] Student student)
    {
        try
        {
            ValidateEnrollmentDate(student.EnrollmentDate);

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            Db.Students.Add(student);
            Db.SaveChanges();

            var studentName = $"{student.FirstMidName} {student.LastName}";
            SendEntityNotification("Student", student.ID.ToString(), studentName, EntityOperation.CREATE);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Trace.TraceError($"Error creating student: {ex.Message} | Student: {student?.FirstMidName} {student?.LastName} | EnrollmentDate: {student?.EnrollmentDate} | Stack: {ex.StackTrace}");
            ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            return View(student);
        }
    }

    public IActionResult Edit(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var student = Db.Students.Find(id.Value);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit([Bind("ID,LastName,FirstMidName,EnrollmentDate")] Student student)
    {
        try
        {
            ValidateEnrollmentDate(student.EnrollmentDate);

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            Db.Entry(student).State = EntityState.Modified;
            Db.SaveChanges();

            var studentName = $"{student.FirstMidName} {student.LastName}";
            SendEntityNotification("Student", student.ID.ToString(), studentName, EntityOperation.UPDATE);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Trace.TraceError($"Error editing student: {ex.Message} | Student ID: {student?.ID} | Student: {student?.FirstMidName} {student?.LastName} | EnrollmentDate: {student?.EnrollmentDate} | Stack: {ex.StackTrace}");
            ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            return View(student);
        }
    }

    public IActionResult Delete(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var student = Db.Students.Find(id.Value);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            var student = Db.Students.Find(id);
            if (student is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var studentName = $"{student.FirstMidName} {student.LastName}";
            Db.Students.Remove(student);
            Db.SaveChanges();

            SendEntityNotification("Student", id.ToString(), studentName, EntityOperation.DELETE);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Trace.TraceError($"Error deleting student: {ex.Message} | Student ID: {id} | Stack: {ex.StackTrace}");
            TempData["ErrorMessage"] = "Unable to delete the student. Try again, and if the problem persists see your system administrator.";
            return RedirectToAction(nameof(Index));
        }
    }

    private void ValidateEnrollmentDate(DateTime enrollmentDate)
    {
        if (enrollmentDate == DateTime.MinValue || enrollmentDate == default)
        {
            ModelState.AddModelError("EnrollmentDate", "Please enter a valid enrollment date.");
        }

        if (enrollmentDate < new DateTime(1753, 1, 1) || enrollmentDate > new DateTime(9999, 12, 31))
        {
            ModelState.AddModelError("EnrollmentDate", "Enrollment date must be between 1753 and 9999.");
        }
    }
}
