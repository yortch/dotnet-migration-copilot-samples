using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly IWebHostEnvironment _environment;

        public CoursesController(
            SchoolContext db,
            NotificationService notificationService,
            IWebHostEnvironment environment)
            : base(db, notificationService)
        {
            _environment = environment;
        }

        // GET: Courses
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Department);
            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course course = db.Courses.Include(c => c.Department).SingleOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            return View(new Course());
        }

        [HttpGet("/Uploads/TeachingMaterials/{fileName}")]
        public ActionResult TeachingMaterialImage(string fileName)
        {
            if (Path.GetFileName(fileName) != fileName)
            {
                return NotFound();
            }

            var contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                _ => null
            };
            if (contentType == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(GetUploadsPath(), fileName);
            return System.IO.File.Exists(filePath)
                ? PhysicalFile(filePath, contentType)
                : NotFound();
        }

        // POST: Courses/Create
        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("CourseID,Title,Credits,DepartmentID")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload if an image is provided
                if (teachingMaterialImage != null && teachingMaterialImage.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(teachingMaterialImage.FileName).ToLowerInvariant();
                    
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("teachingMaterialImage", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
                        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }

                    // Validate file size (max 5MB)
                    if (teachingMaterialImage.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("teachingMaterialImage", "File size must be less than 5MB.");
                        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }

                    try
                    {
                        // Create uploads directory if it doesn't exist
                        var uploadsPath = GetUploadsPath();
                        Directory.CreateDirectory(uploadsPath);

                        // Generate unique filename
                        var fileName = $"course_{course.CourseID}_{Guid.NewGuid()}{fileExtension}";
                        var filePath = Path.Combine(uploadsPath, fileName);

                        // Save file
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            teachingMaterialImage.CopyTo(stream);
                        }
                        course.TeachingMaterialImagePath = $"~/Uploads/TeachingMaterials/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error uploading course image: {ex.Message}");
                        ModelState.AddModelError("teachingMaterialImage", "Unable to upload the image. Please try again.");
                        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }
                }

                db.Courses.Add(course);
                db.SaveChanges();
                
                // Send notification for course creation
                SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.CREATE);
                
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("CourseID,Title,Credits,DepartmentID")] Course course, IFormFile teachingMaterialImage)
        {
            var existingCourse = db.Courses.Find(course.CourseID);
            if (existingCourse == null)
            {
                return NotFound();
            }
            course.TeachingMaterialImagePath = existingCourse.TeachingMaterialImagePath;

            if (ModelState.IsValid)
            {
                // Handle file upload if a new image is provided
                if (teachingMaterialImage != null && teachingMaterialImage.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(teachingMaterialImage.FileName).ToLowerInvariant();
                    
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("teachingMaterialImage", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
                        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }

                    // Validate file size (max 5MB)
                    if (teachingMaterialImage.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("teachingMaterialImage", "File size must be less than 5MB.");
                        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }

                    try
                    {
                        // Create uploads directory if it doesn't exist
                        var uploadsPath = GetUploadsPath();
                        Directory.CreateDirectory(uploadsPath);

                        // Generate unique filename
                        var fileName = $"course_{course.CourseID}_{Guid.NewGuid()}{fileExtension}";
                        var filePath = Path.Combine(uploadsPath, fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            teachingMaterialImage.CopyTo(stream);
                        }
                        var oldFilePath = GetUploadPath(existingCourse.TeachingMaterialImagePath);
                        existingCourse.TeachingMaterialImagePath = $"~/Uploads/TeachingMaterials/{fileName}";
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error deleting replaced course image: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error uploading course image: {ex.Message}");
                        ModelState.AddModelError("teachingMaterialImage", "Unable to upload the image. Please try again.");
                        ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }
                }

                existingCourse.Title = course.Title;
                existingCourse.Credits = course.Credits;
                existingCourse.DepartmentID = course.DepartmentID;
                db.SaveChanges();
                
                // Send notification for course update
                SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.UPDATE);
                
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            course.TeachingMaterialImagePath = existingCourse.TeachingMaterialImagePath;
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course course = db.Courses.Include(c => c.Department).SingleOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseTitle = course.Title;
            
            var imagePath = course.TeachingMaterialImagePath;
            db.Courses.Remove(course);
            db.SaveChanges();

            if (!string.IsNullOrEmpty(imagePath))
            {
                var filePath = GetUploadPath(imagePath);
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error deleting course image: {ex.Message}");
                }
            }
            
            // Send notification for course deletion
            SendEntityNotification("Course", id.ToString(), courseTitle, EntityOperation.DELETE);
            
            return RedirectToAction("Index");
        }

        private string GetUploadsPath()
        {
            return Path.Combine(_environment.ContentRootPath, "Uploads", "TeachingMaterials");
        }

        private string GetUploadPath(string relativePath)
        {
            return Path.Combine(GetUploadsPath(), Path.GetFileName(relativePath));
        }
    }
}
