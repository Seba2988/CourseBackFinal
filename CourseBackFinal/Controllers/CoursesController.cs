using Microsoft.AspNetCore.Http;
using CourseBackFinal.Repositories;
using CourseBackFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using CourseBackFinal.Helpers;

namespace CourseBackFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        public CoursesController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById([FromRoute] int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpGet("")]
        [Authorize(Roles = "Professor")]

        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllCourses();
            if (courses.Count() == 0) return NoContent();
            return Ok(courses);
        }

        [HttpPost("")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> AddCourse([FromBody] CourseModel courseModel)
        {
            object result = await _courseRepository.AddCourse(courseModel);
            return ResponseHandler(result);
        }

        [HttpPost("{courseId}/students/{studentId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> AddStudentToCourse([FromRoute] int courseId, [FromRoute] string studentId)
        {
            object result = await _courseRepository.AddStudentToCourse(courseId, studentId);
            return ResponseHandler(result);
        }
        private IActionResult ResponseHandler(object result)
        {
            if (result.GetType().GetProperty("message") != null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPatch("{courseId}/students/{studentId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> DeleteStudentFromCourse([FromRoute] int courseId, [FromRoute] string studentId)
        {
            object result = await _courseRepository.DeleteStudentFromCourse(courseId, studentId);
            return ResponseHandler(result);
        }
    }
}
