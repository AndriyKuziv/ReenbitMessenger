using Microsoft.AspNetCore.Mvc;
using ReenbitMessenger.Infrastructure.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using ReenbitMessenger.DataAccess.Utils;
using AutoMapper;

namespace ReenbitMessenger.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupChatController : Controller
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly IMapper _mapper;

        public GroupChatController(HandlersDispatcher handlersDispatcher, IMapper mapper)
        {
            _handlersDispatcher = handlersDispatcher;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroupChats()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("userGroupChats")]
        public async Task<IActionResult> GetUserGroupChats()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetGroupChatById([FromRoute]Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupChat()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteGroupChatById([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{id:guid}/updateInfo")]
        public async Task<IActionResult> UpdateGroupChatInfoById([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{id:guid}/addUsers")]
        public async Task<IActionResult> AddUsersToGroupChat([FromRoute] Guid id, 
            [FromBody] AddUsersToGroupRequest addUsersRequest)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id:guid}/removeUsers")]
        public async Task<IActionResult> RemoveUsersFromGroupChat([FromRoute] Guid id,
            [FromBody] RemoveUsersFromGroupRequest removeUsersRequest)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{id:guid}/send")]
        public async Task<IActionResult> SendMessageToGroupChat([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id:guid}/deleteMessage")]
        public async Task<IActionResult> DeleteMessageFromGroupChat([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
