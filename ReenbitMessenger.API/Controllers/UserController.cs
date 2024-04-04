using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class UsersController : Controller
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly IMapper _mapper;

        public UsersController(HandlersDispatcher commandHandlersDispatcher,
            IMapper mapper)
        {
            _handlersDispatcher = commandHandlersDispatcher;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _unitOfWork.Users.GetAllAsync();

        //    var usersDTO = _mapper.Map<List<User>>(users);

        //    return Ok(usersDTO);
        //}

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);

            User result = await _handlersDispatcher.Dispatch(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest createUserRequest)
        {
            var command = new CreateUserCommand(
                createUserRequest.Username,
                createUserRequest.Email,
                createUserRequest.Password);

            var result = await _handlersDispatcher.Dispatch(command);

            if (!result) return BadRequest();

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
