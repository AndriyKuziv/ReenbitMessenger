using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ReenbitMessenger.DataAccess.AppServices.Queries;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly ITokenHandler _tokenHandler;

        public AuthController(HandlersDispatcher handlersDispatcher,
            ITokenHandler tokenHandler)
        {
            _handlersDispatcher = handlersDispatcher;
            _tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn([FromBody]LoginRequest loginRequest)
        {
            var query = new LogInQuery(loginRequest.Email, loginRequest.Password);

            var user = await _handlersDispatcher.Dispatch(query);

            var token = await _tokenHandler.CreateTokenAsync(user);

            return Ok(token);
        }
    }
}
