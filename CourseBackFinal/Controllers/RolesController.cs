using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseBackFinal.Models;
using CourseBackFinal.Repositories;

namespace CourseBackFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesRepository _rolesRepository;
        public RolesController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> NewRole(string roleName)
        {
            var result = await _rolesRepository.CreateRole(roleName);
            if(string.IsNullOrEmpty(roleName))
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
