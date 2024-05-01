using Microsoft.AspNetCore.Mvc;

namespace ReenbitMessenger.API.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> TestConnection()
        {
            return Ok("Healthy");
        }
    }
}
