using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers;

public class InstructorsController : BaseController
{
    public InstructorsController(SchoolContext db, NotificationService notificationService)
        : base(db, notificationService)
    {
    }

    public IActionResult Index(int? id, int? courseID)
    {
        var viewModel = new InstructorIndexData
        {
            Instructors = Db.Instructors
                .AsNoTracking()
                .Include(instructor => instructor.OfficeAssignment)
                .Include(instructor => instructor.CourseAssignments)
                    .ThenInclude(assignment => assignment.Course)
                        .ThenInclude(course => course.Department)
                .OrderBy(instructor => instructor.LastName)
                .ToList()
        };

        if (id is not null)
        {
            ViewData["InstructorID"] = id.Value;
            viewModel.Courses = viewModel.Instructors
                .Where(instructor => instructor.ID == id.Value)
                .Single()
                .CourseAssignments
                .Select(assignment => assignment.Course)
                .ToList();
        }

        if (courseID is not null && viewModel.Courses is not null)
        {
            ViewData["CourseID"] = courseID.Value;
            viewModel.Enrollments = viewModel.Courses
                .Where(course => course.CourseID == courseID.Value)
                .Single()
                .Enrollments;
        }

        return View(viewModel);
    }

    public IActionResult Details(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var instructor = Db.Instructors.Find(id.Value);
        return instructor is null ? NotFound() : View(instructor);
    }

    public IActionResult Create()
    {
        var instructor = new Instructor
        {
            CourseAssignments = []
        };

        PopulateAssignedCourseData(instructor);
        return View(instructor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
    {
        if (selectedCourses is not null)
        {
            instructor.CourseAssignments = [];
            foreach (var course in selectedCourses)
            {
                instructor.CourseAssignments.Add(new CourseAssignment
                {
                    InstructorID = instructor.ID,
                    CourseID = int.Parse(course)
                });
            }
        }

        if (!ModelState.IsValid)
        {
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        Db.Instructors.Add(instructor);
        Db.SaveChanges();
        SendEntityNotification("Instructor", instructor.ID.ToString(), EntityOperation.CREATE);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var instructor = Db.Instructors
            .Include(item => item.OfficeAssignment)
            .Include(item => item.CourseAssignments)
                .ThenInclude(assignment => assignment.Course)
            .SingleOrDefault(item => item.ID == id.Value);

        if (instructor is null)
        {
            return NotFound();
        }

        PopulateAssignedCourseData(instructor);
        return View(instructor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var instructorToUpdate = Db.Instructors
            .Include(item => item.OfficeAssignment)
            .Include(item => item.CourseAssignments)
                .ThenInclude(assignment => assignment.Course)
            .SingleOrDefault(item => item.ID == id.Value);

        if (instructorToUpdate is null)
        {
            return NotFound();
        }

        instructorToUpdate.OfficeAssignment ??= new OfficeAssignment();

        if (await TryUpdateModelAsync(
                instructorToUpdate,
                string.Empty,
                item => item.LastName,
                item => item.FirstMidName,
                item => item.HireDate,
                item => item.OfficeAssignment))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }

                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                Db.SaveChanges();
                SendEntityNotification("Instructor", instructorToUpdate.ID.ToString(), EntityOperation.UPDATE);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }

        PopulateAssignedCourseData(instructorToUpdate);
        return View(instructorToUpdate);
    }

    public IActionResult Delete(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        var instructor = Db.Instructors.Find(id.Value);
        return instructor is null ? NotFound() : View(instructor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var instructor = Db.Instructors
            .Include(item => item.OfficeAssignment)
            .SingleOrDefault(item => item.ID == id);

        if (instructor is null)
        {
            return RedirectToAction(nameof(Index));
        }

        Db.Instructors.Remove(instructor);

        var department = Db.Departments.SingleOrDefault(item => item.InstructorID == id);
        if (department is not null)
        {
            department.InstructorID = null;
        }

        Db.SaveChanges();
        SendEntityNotification("Instructor", id.ToString(), EntityOperation.DELETE);

        return RedirectToAction(nameof(Index));
    }

    private void PopulateAssignedCourseData(Instructor instructor)
    {
        var instructorCourses = new HashSet<int>(
            instructor.CourseAssignments?.Select(assignment => assignment.CourseID) ?? []);

        ViewData["Courses"] = Db.Courses
            .AsNoTracking()
            .OrderBy(course => course.Title)
            .Select(course => new AssignedCourseData
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.CourseID)
            })
            .ToList();
    }

    private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
    {
        if (selectedCourses is null)
        {
            instructorToUpdate.CourseAssignments = [];
            return;
        }

        var selectedCoursesSet = new HashSet<string>(selectedCourses);
        var instructorCourses = new HashSet<int>(
            instructorToUpdate.CourseAssignments.Select(assignment => assignment.CourseID));

        foreach (var course in Db.Courses)
        {
            if (selectedCoursesSet.Contains(course.CourseID.ToString()))
            {
                if (!instructorCourses.Contains(course.CourseID))
                {
                    instructorToUpdate.CourseAssignments.Add(new CourseAssignment
                    {
                        InstructorID = instructorToUpdate.ID,
                        CourseID = course.CourseID
                    });
                }
            }
            else if (instructorCourses.Contains(course.CourseID))
            {
                var courseToRemove = instructorToUpdate.CourseAssignments
                    .SingleOrDefault(assignment => assignment.CourseID == course.CourseID);

                if (courseToRemove is not null)
                {
                    Db.Entry(courseToRemove).State = EntityState.Deleted;
                }
            }
        }
    }
}
