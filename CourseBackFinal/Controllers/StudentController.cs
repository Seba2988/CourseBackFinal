using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseBackFinal.Models;
using CourseBackFinal.Repositories;
using CourseBackFinal.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CourseBackFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ResponseHelper _responseHelper;
        public StudentController(
            IAccountRepository accountRepository,
            IAttendanceRepository attendanceRepository,
            ICourseRepository courseRepository,
            ResponseHelper responseHelper)
        {
            _accountRepository = accountRepository;
            _attendanceRepository = attendanceRepository;
            _courseRepository = courseRepository;
            _responseHelper = responseHelper;
        }

        [HttpGet("logout")]
        [Authorize(Roles = "Student")]
        public async Task Logout()
        {
            await _accountRepository.Logout();
        }

        [HttpGet("{studentId}/absences/{courseId}")]
        [Authorize(Roles = "Student, Professor")]
        public async Task<IActionResult> GetAllAbsencesForCourse([FromRoute] string studentId, [FromRoute] int courseId)
        {
            var result = await _attendanceRepository.GetAbsencesForStudentForCourse(courseId, studentId);
            return _responseHelper.ResponseHandler(result);
        }
        [HttpGet("{studentId}/absences/{courseId}/count")]
        public async Task<IActionResult> GetAbsenceCount([FromRoute] string studentId, [FromRoute] int courseId)
        {
            var result = await _attendanceRepository.GetAbsencesCountForStudentForCourse(courseId, studentId);
            return _responseHelper.ResponseHandler(result);
        }

        [HttpGet]
        [Route("{studentId}/absences/{courseId}/count/{start?}/{end?}")]
        [Route("{studentId}/absences/{courseId}/count/null/{end?}")]
        public async Task<IActionResult> GetAbsenceCount(
            [FromRoute] string studentId,
            [FromRoute] int courseId, 
            [FromRoute] DateTime? start = null, 
            [FromRoute] DateTime? end = null)
        {
            var result = await _attendanceRepository.GetAbsencesCountForStudentForCourse(courseId, studentId, start, end);
            return _responseHelper.ResponseHandler(result);
        }

        [HttpGet("{studentId}/courses")]
        [Authorize(Roles = "Student, Professor")]
        public async Task<IActionResult> GetAllCoursesForStudent([FromRoute] string studentId)
        {
            var result = await _courseRepository.GetAllCoursesForStudent(studentId);
            return _responseHelper.ResponseHandler(result);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupModel signupModel)
        {
            var result = await _accountRepository.SignUp(signupModel, false);
            return _responseHelper.ResponseHandler(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SigninModel signinModel)
        {
            var result = await _accountRepository.Login(signinModel);
            return _responseHelper.ResponseHandler(result);
        }

        [HttpPatch("me")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EditAccount([FromBody] UpdateUserModel updateUserModel)
        {
            var userName = User.Identity.Name;
            if (userName == null) return BadRequest();
            var result = await _accountRepository.UpdateUser(false, userName, updateUserModel);
            return _responseHelper.ResponseHandler(result);
        }

        [HttpPatch("{studentId}/absences/{absenceId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EditAbsence([FromRoute] string studentId, [FromRoute] int absenceId, [FromBody] EditAbsenceModel editAbsenceModel)
        {
            var result = await _attendanceRepository.EditAbsence(absenceId, studentId, editAbsenceModel);
            return _responseHelper.ResponseHandler(result);
        }

        [HttpGet("notInCourse={courseId}")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GetAllStudentsNotInCourse([FromRoute] int courseId)
        {
            var result = await _courseRepository.GetAllStudentNotInCourse(courseId);
            return _responseHelper.ResponseHandler(result);
        }
    }
}
