using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using CourseBackFinal.Models;
namespace CourseBackFinal.Repositories
{
    public interface ICourseRepository
    {
        Task<IQueryable> GetAllCourses();
        Task<CourseModel> GetCourseById(int id);
        Task<int> AddCourse(CourseModel courseModel);
        Task DeleteCourse(int id);
        Task EditCourse(int id, JsonPatchDocument courseModel);
    }
}
