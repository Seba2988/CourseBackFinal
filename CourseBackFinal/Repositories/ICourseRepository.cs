using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using CourseBackFinal.Models;
using CourseBackFinal.DTO;
namespace CourseBackFinal.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<CourseDTO>> GetAllCourses();
        Task<CourseDTO> GetCourseById(int id);
        Task<object>AddCourse(CourseModel courseModel);

        Task<object> AddStudentToCourse(int courseId, string studentId);
        Task<object> DeleteCourse(int id);
        Task<object> DeleteStudentFromCourse(int courseId, string studentId);
        Task<object> GetAllStudentNotInCourse(int courseId);
        Task<IEnumerable<CourseInStudentDTO>> GetAllCoursesForStudent(string studentId);
    }
}
