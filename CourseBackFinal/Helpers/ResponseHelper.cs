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
                if (result.Code == 201) return NoContent();
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
