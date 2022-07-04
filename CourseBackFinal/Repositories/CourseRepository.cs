using CourseBackFinal.Models;
using CourseBackFinal.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using CourseBackFinal.DTO;
using CourseBackFinal.Helpers;

namespace CourseBackFinal.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseManagementContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CourseRepository(
            CourseManagementContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCourses()
        {
            var courses = await ContextHelper.QueryCourses(_context).ToListAsync();
            return courses;
        }
        public async Task<CourseDTO> GetCourseById(int id)
        {
            var course = await ContextHelper.QueryCourses(_context).FirstOrDefaultAsync(c => c.Id == id);
            return course;
        }
        public async Task<object> AddCourse(CourseModel courseModel)
        {
            var currentCourse = _context.Courses.FirstOrDefault(c => c.Name == courseModel.Name);
            if (currentCourse != null)
            {
                return new
                {
                    code = 409,
                    message = "This course already exists"
                };
            }
            var course = new CourseModel()
            {
                Name = courseModel.Name,
                StartingDate = courseModel.StartingDate,
                EndingDate = courseModel.EndingDate,
                ProfessorId = courseModel.ProfessorId,
            };
            List<ClassModel> classes = new();
            DateTime start = courseModel.StartingDate;
            DateTime end = courseModel.EndingDate;

            while (start.AddDays(7) <= end)
            {
                start = start.AddDays(7);
                classes.Add(
                    new ClassModel()
                    {
                        Course = course,
                        Date = start,
                    });
            }
            _context.Courses.Add(course);
            _context.Classes.AddRange(classes);
            var result = await _context.SaveChangesAsync();
            if (result != 0) return new
            {
                code = 201,
                course.Id
            };
            return new
            {
                code = 400,
                message = "No course has been created"
            };
        }
        public async Task<object> DeleteCourse(int id)
        {
            var course = await ContextHelper.CourseGetter(id, _context);
            if (course == null)
            {
                return new
                {
                    code = 400,
                    message = $"No course was found with id: {id}"
                };
            }
            var absences = await _context.Absences.Where(a => a.Class.Course.Id == id).ToListAsync();
            _context.Absences.RemoveRange(absences);
            _context.Courses.Remove(course);
            return await _context.SaveChangesAsync();
        }
        public async Task<object> AddStudentToCourse(int courseId, string studentId)
        {
            var course = await ContextHelper.CourseGetter(courseId, _context);
            if (course == null)
            {
                return new
                {
                    code = 400,
                    message = "Course not found"
                };
            }
            var student = await _userManager.FindByIdAsync(studentId);
            if (student == null)
            {
                return new
                {
                    code = 400,
                    message = "Student not found"
                };
            }
            foreach (var classId in course.Classes)
            {
                var absence = new AbsenceModel()
                {
                    Class = classId,
                    Student = student,
                    IsPresent = false
                };
                classId.Absences.Add(absence);
            }
            course.Students.Add(student);
            await _context.SaveChangesAsync();
            return new
            {
                code = 200,
            };
        }

        public async Task<object> DeleteStudentFromCourse(int courseId, string studentId)
        {
            var course = await ContextHelper.CourseGetter(courseId, _context);
            if (course == null)
            {
                return new
                {
                    code = 400,
                    message = "Course not found"
                };
            }
            var student = course.Students.SingleOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return new
                {
                    code = 400,
                    message = "This student is not in this course"
                };
            }
            var absencesForStudent = student.Absences.Where(a => a.Class.Course.Id == courseId).ToList();
            _context.Absences.RemoveRange(absencesForStudent);
            course.Students.Remove(student);
            await _context.SaveChangesAsync();
            return new
            {
                code = 200
            };
        }

        public async Task<object> GetAllStudentNotInCourse(int courseId)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                return new
                {
                    code = 400,
                    message = "Course not found"
                };
            }
            var students = await _context.Users
                .Where(u => u.Address != null)
                .Where(u => u.Courses.All(c => c.Id != courseId))
                .Select(u => new StudentInCourseDTO
                {
                    Address = u.Address,
                    Id = u.Id,
                    DateOfBirth = u.DateOfBirth,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).ToListAsync();
            if (students.Count == 0)
            {
                return new
                {
                    code = 400,
                    message = "No students to add to this course"
                };
            }
            return students;
        }

        public async Task<IEnumerable<CourseInStudentDTO>> GetAllCoursesForStudent(string studentId)
        {
            var courses = await _context.Courses
                .Where(c => c.Students
                .Any(s => s.Id == studentId))
                .Select(c => new CourseInStudentDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    StartingDate = c.StartingDate,
                    EndingDate = c.EndingDate,
                    ProfessorId = c.ProfessorId
                }).ToListAsync();
            
            return courses;

        }
    }
}
