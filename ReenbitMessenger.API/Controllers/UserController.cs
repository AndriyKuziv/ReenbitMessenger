using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitMessenger.DataAccess.AppServices.Commands;
using ReenbitMessenger.DataAccess.AppServices.Commands.User;
using ReenbitMessenger.DataAccess.AppServices.Commands.User.Validators;
using ReenbitMessenger.DataAccess.AppServices.Queries.User;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsersController : Controller
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly IMapper _mapper;
        private readonly IValidatorsHandler _validatorsHandler;

        public UsersController(HandlersDispatcher commandHandlersDispatcher,
            IMapper mapper, IValidatorsHandler validatorsHandler)
        {
            _handlersDispatcher = commandHandlersDispatcher;
            _mapper = mapper;
            _validatorsHandler = validatorsHandler;
        }

        [HttpPost]
        [Route("usersList")]
        public async Task<IActionResult> GetSortedUsers([FromBody]GetUsersRequest getUsersRequest)
        {
            var query = new GetUsersQuery(
                getUsersRequest.NumberOfUsers,
                getUsersRequest.ValueContains,
                getUsersRequest.Page,
                getUsersRequest.Ascending,
                getUsersRequest.OrderBy);

            var users = await _handlersDispatcher.Dispatch(query);

            var usersDTO = _mapper.Map<List<User>>(users);

            return Ok(usersDTO);
        }

        [HttpGet]
        [Route("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var query = new GetUserByIdQuery(Convert.ToString(userId));

            var user = await _handlersDispatcher.Dispatch(query);

            var userDTO = _mapper.Map<User>(user);

            return Ok(userDTO);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest createUserRequest)
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

            var dispatchResult = await _handlersDispatcher.Dispatch(command);

            if (!dispatchResult) return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("{userId:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            var command = new DeleteUserCommand(Convert.ToString(userId));

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var deleteResult = await _handlersDispatcher.Dispatch(command);

            if (!deleteResult) return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Route("{userId:guid}")]
        public async Task<IActionResult> EditUserInfoById(
            [FromRoute] Guid userId,
            [FromBody] EditUserInfoRequest editUserInfoRequest)
        {
            var command = new EditUserInfoCommand(Convert.ToString(userId),
                editUserInfoRequest.Username, editUserInfoRequest.Email);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var editResult = await _handlersDispatcher.Dispatch(command);

            if (!editResult) return BadRequest();

            return Ok();
        }
    }
}
