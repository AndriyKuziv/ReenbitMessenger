using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitMessenger.DataAccess.AppServices.Commands;
using ReenbitMessenger.DataAccess.AppServices.Queries;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserCommand> _validator;

        public UsersController(HandlersDispatcher commandHandlersDispatcher,
            IMapper mapper, IValidator<CreateUserCommand> validator)
        {
            _handlersDispatcher = commandHandlersDispatcher;
            _mapper = mapper;
            _validator = validator;
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
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);

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

            var result = await _validator.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var dispatchResult = await _handlersDispatcher.Dispatch(command);

            if (!dispatchResult) return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var command = new DeleteUserCommand(id);

            var result = await _handlersDispatcher.Dispatch(command);

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Route("{userId:guid}")]
        public async Task<IActionResult> EditUserInfoById(
            [FromRoute] Guid userId,
            [FromBody] EditUserInfoRequest editUserInfoRequest)
        {
            var command = new EditUserInfoCommand(userId,
                editUserInfoRequest.Email, editUserInfoRequest.Username);

            var result = await _handlersDispatcher.Dispatch(command);

            if (!result) return BadRequest();

            return Ok();
        }
    }
}
