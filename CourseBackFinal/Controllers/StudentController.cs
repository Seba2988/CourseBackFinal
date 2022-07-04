using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseBackFinal.Models;
using CourseBackFinal.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace CourseBackFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        public StudentController(
            IAccountRepository accountRepository, 
            IAttendanceRepository attendanceRepository
            )
        {
            _accountRepository = accountRepository;
            _attendanceRepository = attendanceRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupModel signupModel)
        {
            var result = await _accountRepository.SignUp(signupModel, false);
            if (result == null || !result.Succeeded) return BadRequest(result);
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
        [Authorize(Roles = "Student")]
        public async Task Logout()
        {
            await _accountRepository.Logout();
        }

        [HttpGet("{studentId}/absences/{courseId}")]
        public async Task<IActionResult> GetAllAbsencesForCourse([FromRoute] string studentId, [FromRoute] int courseId)
        {
            var result = await _attendanceRepository.GetAbsencesForStudentForCourse(courseId, studentId);
            if (result.Count() == 0) return BadRequest(result);
            return Ok(result);
        }
    }
}
