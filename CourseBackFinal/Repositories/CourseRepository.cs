using CourseBackFinal.Models;
using CourseBackFinal.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
namespace CourseBackFinal.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseManagementContext _context;
        public CourseRepository(CourseManagementContext context)
        {
            _context = context;
        }

        public Task<IQueryable> GetAllCourses()
        {
            var courses = _context.Courses;
            return (Task<IQueryable>)(IQueryable)courses;
        }
        public async Task<CourseModel> GetCourseById(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            return course;
        }

        public async Task<int> AddCourse(CourseModel courseModel)
        {
            var course = new CourseModel()
            {
                Name = courseModel.Name,
                StartingDate = courseModel.StartingDate,
                EndingDate = courseModel.EndingDate,
                DayOfWeek = courseModel.DayOfWeek,
                Hour = courseModel.Hour
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course.Id;
        }
        public async Task EditCourse(int id, JsonPatchDocument courseModel)
        {
            var course = await _context.Courses.FindAsync(id);
            if(course != null)
            {
                courseModel.ApplyTo(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCourse(int id)
        {
            var course = new CourseModel() { Id = id };
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
