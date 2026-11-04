using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : BaseController
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private const long MaxImageBytes = 5 * 1024 * 1024;

        private readonly IWebHostEnvironment env;

        public CoursesController(SchoolContext context, NotificationService notificationService, IWebHostEnvironment env)
            : base(context, notificationService)
        {
            this.env = env;
        }

        private string UploadsPath => Path.Combine(env.ContentRootPath, "Uploads", "TeachingMaterials");

        // Maps a stored "~/Uploads/..." path to a physical path.
        private string MapPath(string virtualPath)
        {
            var relative = virtualPath.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(env.ContentRootPath, relative);
        }

        private void PopulateDepartments(object selected = null)
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", selected);
        }

        // Validates and saves the uploaded image. Returns the stored path, or null with a model error added.
        private string SaveImage(IFormFile image, int courseId, string oldPath)
        {
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("teachingMaterialImage", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
                return null;
            }

            if (image.Length > MaxImageBytes)
            {
                ModelState.AddModelError("teachingMaterialImage", "File size must be less than 5MB.");
                return null;
            }

            try
            {
                Directory.CreateDirectory(UploadsPath);

                var fileName = $"course_{courseId}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(UploadsPath, fileName);

                if (!string.IsNullOrEmpty(oldPath))
                {
                    var oldFilePath = MapPath(oldPath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                return $"~/Uploads/TeachingMaterials/{fileName}";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("teachingMaterialImage", "Error uploading file: " + ex.Message);
                return null;
            }
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
            Course course = db.Courses.Include(c => c.Department).Where(c => c.CourseID == id).SingleOrDefault();
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            PopulateDepartments();
            return View(new Course());
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                if (teachingMaterialImage != null && teachingMaterialImage.Length > 0)
                {
                    var savedPath = SaveImage(teachingMaterialImage, course.CourseID, null);
                    if (savedPath == null)
                    {
                        PopulateDepartments(course.DepartmentID);
                        return View(course);
                    }
                    course.TeachingMaterialImagePath = savedPath;
                }

                db.Courses.Add(course);
                db.SaveChanges();

                SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.CREATE);

                return RedirectToAction("Index");
            }

            PopulateDepartments(course.DepartmentID);
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
            PopulateDepartments(course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                if (teachingMaterialImage != null && teachingMaterialImage.Length > 0)
                {
                    var savedPath = SaveImage(teachingMaterialImage, course.CourseID, course.TeachingMaterialImagePath);
                    if (savedPath == null)
                    {
                        PopulateDepartments(course.DepartmentID);
                        return View(course);
                    }
                    course.TeachingMaterialImagePath = savedPath;
                }

                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();

                SendEntityNotification("Course", course.CourseID.ToString(), course.Title, EntityOperation.UPDATE);

                return RedirectToAction("Index");
            }
            PopulateDepartments(course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course course = db.Courses.Include(c => c.Department).Where(c => c.CourseID == id).SingleOrDefault();
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

            if (!string.IsNullOrEmpty(course.TeachingMaterialImagePath))
            {
                var filePath = MapPath(course.TeachingMaterialImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error deleting file: {ex.Message}");
                    }
                }
            }

            db.Courses.Remove(course);
            db.SaveChanges();

            SendEntityNotification("Course", id.ToString(), courseTitle, EntityOperation.DELETE);

            return RedirectToAction("Index");
        }
    }
}
