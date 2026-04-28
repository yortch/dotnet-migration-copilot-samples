using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers;

public class DepartmentsController : BaseController
{
    public DepartmentsController(SchoolContext db, NotificationService notificationService)
        : base(db, notificationService)
    {
    }

    public IActionResult Index()
    {
        var departments = Db.Departments
            .AsNoTracking()
            .Include(department => department.Administrator)
            .ToList();

        return View(departments);
    }

    public IActionResult Details(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var department = Db.Departments.Find(id.Value);
        return department is null ? NotFound() : View(department);
    }

    public IActionResult Create()
    {
        PopulateInstructorDropDownList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Budget,StartDate,InstructorID")] Department department)
    {
        if (!ModelState.IsValid)
        {
            PopulateInstructorDropDownList(department.InstructorID);
            return View(department);
        }

        Db.Departments.Add(department);
        Db.SaveChanges();
        SendEntityNotification("Department", department.DepartmentID.ToString(), department.Name, EntityOperation.CREATE);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var department = Db.Departments.Find(id.Value);
        if (department is null)
        {
            return NotFound();
        }

        PopulateInstructorDropDownList(department.InstructorID);
        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit([Bind("DepartmentID,Name,Budget,StartDate,InstructorID,RowVersion")] Department department)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                PopulateInstructorDropDownList(department.InstructorID);
                return View(department);
            }

            Db.Entry(department).State = EntityState.Modified;
            Db.SaveChanges();
            SendEntityNotification("Department", department.DepartmentID.ToString(), department.Name, EntityOperation.UPDATE);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entry = ex.Entries.Single();
            var clientValues = (Department)entry.Entity;
            var databaseEntry = entry.GetDatabaseValues();

            if (databaseEntry is null)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
            }
            else
            {
                var databaseValues = (Department)databaseEntry.ToObject();

                if (databaseValues.Name != clientValues.Name)
                {
                    ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                }

                if (databaseValues.Budget != clientValues.Budget)
                {
                    ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");
                }

                if (databaseValues.StartDate != clientValues.StartDate)
                {
                    ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate:d}");
                }

                if (databaseValues.InstructorID != clientValues.InstructorID)
                {
                    var instructor = Db.Instructors.Find(databaseValues.InstructorID);
                    ModelState.AddModelError("InstructorID", $"Current value: {instructor?.FullName}");
                }

                ModelState.AddModelError(
                    string.Empty,
                    "The record you attempted to edit was modified by another user after you got the original value. " +
                    "The edit operation was canceled and the current values in the database have been displayed. " +
                    "If you still want to edit this record, click Save again.");

                department.RowVersion = databaseValues.RowVersion;
            }
        }

        PopulateInstructorDropDownList(department.InstructorID);
        return View(department);
    }

    public IActionResult Delete(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var department = Db.Departments.Find(id.Value);
        return department is null ? NotFound() : View(department);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var department = Db.Departments.Find(id);
        if (department is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var departmentName = department.Name;
        Db.Departments.Remove(department);
        Db.SaveChanges();
        SendEntityNotification("Department", id.ToString(), departmentName, EntityOperation.DELETE);

        return RedirectToAction(nameof(Index));
    }

    private void PopulateInstructorDropDownList(object selectedInstructor = null)
    {
        ViewData["InstructorID"] = new SelectList(
            Db.Instructors.AsNoTracking().OrderBy(instructor => instructor.LastName),
            "ID",
            "FullName",
            selectedInstructor);
    }
}
