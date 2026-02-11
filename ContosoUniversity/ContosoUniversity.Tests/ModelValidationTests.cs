using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ContosoUniversity.Models;
using NUnit.Framework;

namespace ContosoUniversity.Tests
{
    [TestFixture]
    public class ModelValidationTests
    {
        [Test]
        public void PersonFullNameUsesLastNameAndFirstName()
        {
            var person = new TestPerson
            {
                LastName = "Doe",
                FirstMidName = "Jane"
            };

            Assert.That(person.FullName, Is.EqualTo("Doe, Jane"));
        }

        [Test]
        public void StudentEnrollmentDateMustBeWithinRange()
        {
            var student = new Student
            {
                FirstMidName = "Jenna",
                LastName = "Lee",
                EnrollmentDate = new DateTime(1600, 1, 1)
            };

            var results = ValidateModel(student);

            Assert.That(results.Any(result => result.MemberNames.Contains(nameof(Student.EnrollmentDate))), Is.True);
        }

        [Test]
        public void StudentEnrollmentDateWithinRangeIsValid()
        {
            var student = new Student
            {
                FirstMidName = "Jenna",
                LastName = "Lee",
                EnrollmentDate = new DateTime(2020, 1, 1)
            };

            var results = ValidateModel(student);

            Assert.That(results, Is.Empty);
        }

        [Test]
        public void CourseCreditsMustBeWithinRange()
        {
            var course = new Course
            {
                CourseID = 42,
                Title = "Biology",
                Credits = 6,
                DepartmentID = 1
            };

            var results = ValidateModel(course);

            Assert.That(results.Any(result => result.MemberNames.Contains(nameof(Course.Credits))), Is.True);
        }

        [Test]
        public void CourseCreditsWithinRangeAreValid()
        {
            var course = new Course
            {
                CourseID = 44,
                Title = "Biology",
                Credits = 4,
                DepartmentID = 1
            };

            var results = ValidateModel(course);

            Assert.That(results, Is.Empty);
        }

        [Test]
        public void CourseTitleMustMeetMinimumLength()
        {
            var course = new Course
            {
                CourseID = 43,
                Title = "AI",
                Credits = 3,
                DepartmentID = 1
            };

            var results = ValidateModel(course);

            Assert.That(results.Any(result => result.MemberNames.Contains(nameof(Course.Title))), Is.True);
        }

        [Test]
        public void CourseTitleMeetingMinimumLengthIsValid()
        {
            var course = new Course
            {
                CourseID = 45,
                Title = "AI Lab",
                Credits = 3,
                DepartmentID = 1
            };

            var results = ValidateModel(course);

            Assert.That(results, Is.Empty);
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        private sealed class TestPerson : Person
        {
        }
    }
}
