using Microsoft.AspNetCore.Mvc;
using ReenbitMessenger.DataAccess.AppServices.Commands;
using ReenbitMessenger.DataAccess.AppServices.Queries;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.Infrastructure.Models.DTO;

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
            var query = new LogInQuery(loginRequest.Username, loginRequest.Password);
            
            var user = await _handlersDispatcher.Dispatch(query);

            if (user is null)
            {
                return NotFound();
            }

            var token = await _tokenHandler.CreateTokenAsync(user);

            return Ok(token);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest createUserRequest)
        {
            var command = new CreateUserCommand(
                createUserRequest.Username,
                createUserRequest.Email,
                createUserRequest.Password);

            var success = await _handlersDispatcher.Dispatch(command);

            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
