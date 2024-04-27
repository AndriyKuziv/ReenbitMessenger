using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.AppServices.AuthServices;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly ITokenHandler _tokenHandler;
        private readonly IValidatorsHandler _validatorsHandler;

        public AuthController(HandlersDispatcher handlersDispatcher,
            ITokenHandler tokenHandler,
            IValidatorsHandler validatorsHandler)
        {
            _handlersDispatcher = handlersDispatcher;
            _tokenHandler = tokenHandler;
            _validatorsHandler = validatorsHandler;
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

            return Ok(new AuthToken() { Token = token});
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] CreateUserRequest createUserRequest)
        {
            var command = new CreateUserCommand(
                createUserRequest.Username,
                createUserRequest.Email,
                createUserRequest.Password);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var success = await _handlersDispatcher.Dispatch(command);

            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
