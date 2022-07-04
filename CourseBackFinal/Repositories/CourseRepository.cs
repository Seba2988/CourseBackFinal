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

        public async Task DeleteCourse(int id)
        {
            var course = new CourseModel()
            {
                Id = id
            };
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
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

        public async Task<IEnumerable<StudentInCourseDTO>> GetAllStudentNotInCourse(int courseId)
        {
            var students = await _context.Users
                .Select(u => new StudentInCourseDTO
                {
                    Address = u.Address,
                    Id = u.Id,
                    DateOfBirth = u.DateOfBirth,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                }).ToListAsync();

            return students;
        }

        /*private IQueryable<CourseDTO> QueryCourses()
        {
            return _context.Courses
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    ProfessorId = c.ProfessorId,
                    StartingDate = c.StartingDate,
                    Name = c.Name,
                    EndingDate = c.EndingDate,
                    Classes = (IList<ClassDTO>)c.Classes
                    .Select(c => new ClassDTO
                    {
                        Id = c.Id,
                        Date = c.Date
                    }),
                    Students = (IList<StudentInCourseDTO>)c.Students
                    .Select(s => new StudentInCourseDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        DateOfBirth = s.DateOfBirth,
                        FirstName = s.FirstName,
                        LastName = s.LastName
                    })
                });
        }*/
    }
}
