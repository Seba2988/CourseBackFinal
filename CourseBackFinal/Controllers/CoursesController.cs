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
        private readonly ResponseHelper _responseHelper;
        public CoursesController(
            ICourseRepository courseRepository,
            ResponseHelper responseHelper)
        {
            _courseRepository = courseRepository;
            _responseHelper = responseHelper;
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
            if (!courses.Any()) return NoContent();
            return Ok(courses);
        }

        [HttpPost("")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> AddCourse([FromBody] CourseModel courseModel)
        {
            var result = await _courseRepository.AddCourse(courseModel);
            var responseHandler = new ResponseHelper();
            return responseHandler.ResponseHandler(result);
        }

        [HttpPost("{courseId}/students/{studentId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> AddStudentToCourse([FromRoute] int courseId, [FromRoute] string studentId)
        {
            var result = await _courseRepository.AddStudentToCourse(courseId, studentId);
            return _responseHelper.ResponseHandler(result);
        }
        

        [HttpPatch("{courseId}/students/{studentId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> DeleteStudentFromCourse([FromRoute] int courseId, [FromRoute] string studentId)
        {
            var result = await _courseRepository.DeleteStudentFromCourse(courseId, studentId);
            return _responseHelper.ResponseHandler(result);
        }

        [HttpGet("{courseId}/studentsAvailable")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GetAllStudentsNotInCourse([FromRoute] int courseId)
        {
            var result = await _courseRepository.GetAllStudentNotInCourse(courseId);
            return _responseHelper.ResponseHandler(result);
        }

        [HttpDelete("{courseId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int courseId)
        {
            var result = await _courseRepository.DeleteCourse(courseId);
            return _responseHelper.ResponseHandler(result);
        }



        /*private IActionResult ResponseHandler(ResponseObject result)
        {
            if (result.Message != null)
            {
                return BadRequest(result);
            }
            return Ok(result.Result);
        }*/
    }
}
