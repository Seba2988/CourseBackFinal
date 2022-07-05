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
        Task<ResponseObject>AddCourse(CourseModel courseModel);

        Task<ResponseObject> AddStudentToCourse(int courseId, string studentId);
        Task<ResponseObject> DeleteCourse(int id);
        Task<ResponseObject> DeleteStudentFromCourse(int courseId, string studentId);
        Task<ResponseObject> GetAllStudentNotInCourse(int courseId);
        Task<ResponseObject> GetAllCoursesForStudent(string studentId);
        Task<ResponseObject> GetAllCoursesForProfessor(string professorId);
    }
}
