using CourseBackFinal.Models;
using CourseBackFinal.Data;
using CourseBackFinal.DTO;
using Microsoft.EntityFrameworkCore;

namespace CourseBackFinal.Helpers
{
    public class ContextHelper
    {
        public static async Task<CourseModel> CourseGetter(int courseId, CourseManagementContext _context)
        {
            return await _context.Courses.Include(c => c.Students).Include(c => c.Classes).ThenInclude(c => c.Absences).SingleOrDefaultAsync(c => c.Id == courseId);
        }
        public static IQueryable<AbsenceDTO> AbsencesGetter(int courseId, string studentId, CourseManagementContext _context)
        {
            return _context.Absences
                .Where(a => a.Class.Course.Id == courseId)
                .Where(a => a.Student.Id == studentId)
                .Select(a => new AbsenceDTO
                {
                    Id = a.Id,
                    StudentId = studentId,
                    ReasonOfAbsence = a.ReasonOfAbsence,
                    Class = new ClassDTO
                    {
                        Id = a.Class.Id,
                        Date = a.Class.Date
                    },
                    IsPresent = a.IsPresent
                });
        }
        public static IQueryable<CourseDTO> QueryCourses(CourseManagementContext _context)
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
                    .OrderBy(s=>s.LastName)
                    .OrderBy(s=>s.FirstName)
                });
        }
    }
}
