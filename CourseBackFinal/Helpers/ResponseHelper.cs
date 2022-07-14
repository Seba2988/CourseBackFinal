using Microsoft.AspNetCore.Mvc;
using CourseBackFinal.Models;


namespace CourseBackFinal.Helpers
{
    public class ResponseHelper : ControllerBase
    {
        public IActionResult ResponseHandler(ResponseObject result)
        {
            if (result.Message != null)
            {
                if (result.Code == 204) return NoContent();
                if (result.Code == 401) return Unauthorized(result);
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
