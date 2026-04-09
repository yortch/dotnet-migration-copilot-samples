using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers;

public class CoursesController : BaseController
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(
        SchoolContext db,
        NotificationService notificationService,
        IWebHostEnvironment environment,
        ILogger<CoursesController> logger)
        : base(db, notificationService)
    {
        _environment = environment;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var courses = Db.Courses
            .AsNoTracking()
            .Include(course => course.Department)
            .ToList();

        return View(courses);
    }

    public IActionResult Details(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var course = Db.Courses
            .AsNoTracking()
            .Include(item => item.Department)
            .SingleOrDefault(item => item.CourseID == id.Value);

        return course is null ? NotFound() : View(course);
    }

    public IActionResult Create()
    {
        PopulateDepartmentsDropDownList();
        return View(new Course());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
    {
        if (!await TryStoreTeachingMaterialAsync(course, teachingMaterialImage))
        {
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        if (!ModelState.IsValid)
        {
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        Db.Courses.Add(course);
        Db.SaveChanges();
        SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.CREATE);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var course = Db.Courses.Find(id.Value);
        if (course is null)
        {
            return NotFound();
        }

        PopulateDepartmentsDropDownList(course.DepartmentID);
        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
    {
        if (!await TryStoreTeachingMaterialAsync(course, teachingMaterialImage))
        {
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        if (!ModelState.IsValid)
        {
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        Db.Entry(course).State = EntityState.Modified;
        Db.SaveChanges();
        SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.UPDATE);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var course = Db.Courses
            .AsNoTracking()
            .Include(item => item.Department)
            .SingleOrDefault(item => item.CourseID == id.Value);

        return course is null ? NotFound() : View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var course = Db.Courses.Find(id);
        if (course is null)
        {
            return RedirectToAction(nameof(Index));
        }

        DeleteExistingTeachingMaterial(course.TeachingMaterialImagePath);

        var courseTitle = course.Title;
        Db.Courses.Remove(course);
        Db.SaveChanges();
        SendEntityNotification("Course", id.ToString(), courseTitle, EntityOperation.DELETE);

        return RedirectToAction(nameof(Index));
    }

    private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
    {
        ViewData["DepartmentID"] = new SelectList(
            Db.Departments.AsNoTracking().OrderBy(department => department.Name),
            "DepartmentID",
            "Name",
            selectedDepartment);
    }

    private async Task<bool> TryStoreTeachingMaterialAsync(Course course, IFormFile teachingMaterialImage)
    {
        if (teachingMaterialImage is null || teachingMaterialImage.Length == 0)
        {
            return true;
        }

        var fileExtension = Path.GetExtension(teachingMaterialImage.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(fileExtension))
        {
            ModelState.AddModelError("teachingMaterialImage", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
            return false;
        }

        if (teachingMaterialImage.Length > 5 * 1024 * 1024)
        {
            ModelState.AddModelError("teachingMaterialImage", "File size must be less than 5MB.");
            return false;
        }

        try
        {
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads", "TeachingMaterials");
            Directory.CreateDirectory(uploadsPath);

            DeleteExistingTeachingMaterial(course.TeachingMaterialImagePath);

            var fileName = $"course_{course.CourseID}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await teachingMaterialImage.CopyToAsync(stream);

            course.TeachingMaterialImagePath = $"/Uploads/TeachingMaterials/{fileName}";
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading teaching material for course {CourseId}", course.CourseID);
            ModelState.AddModelError("teachingMaterialImage", $"Error uploading file: {ex.Message}");
            return false;
        }
    }

    private void DeleteExistingTeachingMaterial(string existingPath)
    {
        if (string.IsNullOrWhiteSpace(existingPath))
        {
            return;
        }

        var relativePath = existingPath.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
        var filePath = Path.Combine(_environment.ContentRootPath, relativePath);
        if (!System.IO.File.Exists(filePath))
        {
            return;
        }

        try
        {
            System.IO.File.Delete(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Unable to delete teaching material file {FilePath}", filePath);
        }
    }
}
