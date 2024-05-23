using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.AppServices.UserServices.Queries;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;

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

            if (user is null)
            {
                return BadRequest("There is no user with such id.");
            }

            var userDTO = _mapper.Map<User>(user);

            return Ok(userDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userId = await ControllerHelper.GetUserId(HttpContext);

            var query = new GetUserByIdQuery(Convert.ToString(userId));

            var user = await _handlersDispatcher.Dispatch(query);

            if (user is null)
            {
                return BadRequest("There is no user with such id.");
            }

            var userDTO = _mapper.Map<User>(user);

            return Ok(userDTO);
        }

        [HttpDelete]
        [Route("{userId:guid}")]
        public async Task<IActionResult> DeleteUserById([FromRoute] Guid userId)
        {
            var command = new DeleteUserCommand(Convert.ToString(userId));

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var deleteSuccess = await _handlersDispatcher.Dispatch(command);

            if (!deleteSuccess) return BadRequest();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = await ControllerHelper.GetUserId(HttpContext);

            var command = new DeleteUserCommand(userId);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var deleteSuccess = await _handlersDispatcher.Dispatch(command);

            if (!deleteSuccess) return BadRequest();

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

            if (editResult is null)
            {
                return BadRequest("Error during editing user.");
            }

            var editResultDTO = _mapper.Map<User>(editResult);

            return Ok(editResultDTO);
        }

        [HttpPut]
        public async Task<IActionResult> EditUserInfo(
            [FromBody] EditUserInfoRequest editUserInfoRequest)
        {
            var userId = await ControllerHelper.GetUserId(HttpContext);

            var command = new EditUserInfoCommand(userId,
                editUserInfoRequest.Username, editUserInfoRequest.Email);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var editResult = await _handlersDispatcher.Dispatch(command);

            if (editResult is null)
            {
                return BadRequest("Error during editing user.");
            }

            var editResultDTO = _mapper.Map<User>(editResult);

            return Ok(editResultDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UploadUserAvatar([FromBody] UploadUserAvatarRequest uploadAvatarRequest)
        {

        }
    }
}
