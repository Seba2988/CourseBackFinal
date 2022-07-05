using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseBackFinal.Models;
using CourseBackFinal.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using CourseBackFinal.Helpers;

namespace CourseBackFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ResponseHelper _responseHelper;
        public ProfessorController(
            IAccountRepository accountRepository,
            ICourseRepository courseRepository,
            ResponseHelper responseHelper)
        {
            _accountRepository = accountRepository;
            _courseRepository = courseRepository;
            _responseHelper = responseHelper;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupModel signupModel)
        {
            var result = await _accountRepository.SignUp(signupModel, true);
            if (result == null || !result.Succeeded) return BadRequest();
            return Ok(result);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SigninModel signinModel)
        {
            var result = await _accountRepository.Login(signinModel);
            if (string.IsNullOrEmpty(result)) return Unauthorized();
            return Ok(result);
        }
        [HttpGet("logout")]
        [Authorize(Roles = "Professor")]
        public async Task Logout()
        {
            await _accountRepository.Logout();
        }

        [HttpPatch("me")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> EditAccount([FromBody] UpdateUserModel updateUserModel)
        {
            var userName = User.Identity.Name;
            var result = await _accountRepository.UpdateUser(true, userName, updateUserModel);
            if (result == null || !result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("students")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _accountRepository.GetAllUsersByRoleName("Student");
            if (students == null) return NoContent();
            return Ok(students);
        }

        [HttpDelete("students/{id}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> DeleteStudent([FromRoute] string id)
        {
            var result = await _accountRepository.DeleteUser(id);
            if (result.Succeeded) return Ok();
            return BadRequest(result);
        }

        [HttpGet("students/{id}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GetStudentById([FromRoute] string id)
        {
            var result = await _accountRepository.GetUserById(id);
            if (result == null) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{professorId}/courses")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GetAllCoursesForProfessor([FromRoute] string professorId)
        {
            var result = await _courseRepository.GetAllCoursesForProfessor(professorId);
            return _responseHelper.ResponseHandler(result);
        }
    }
}
