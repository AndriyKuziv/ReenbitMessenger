using Microsoft.AspNetCore.Mvc;
using ReenbitMessenger.Infrastructure.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using ReenbitMessenger.DataAccess.Utils;
using AutoMapper;
using ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries;
using System.Security.Claims;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;

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
            var query = new GetGroupChatsQuery(0);

            var groupChats = await _handlersDispatcher.Dispatch(query);

            var groupChatsDTO = _mapper.Map<IEnumerable<GroupChat>>(groupChats);

            return Ok(groupChatsDTO);
        }

        [HttpGet]
        [Route("userGroupChats")]
        public async Task<IActionResult> GetUserGroupChats()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return NotFound("Identity is null");
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User id is null");
            }

            var groupChats = await _handlersDispatcher.Dispatch(new GetUserGroupChatsQuery(userId));

            var groupChatsDTO = _mapper.Map<IEnumerable<GroupChat>>(groupChats);

            return Ok(groupChatsDTO);
        }

        [HttpGet]
        [Route("messagesHistory")]
        public async Task<IActionResult> GetUserMessagesHistory()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return NotFound("Identity is null");
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User id is null");
            }

            var messages = await _handlersDispatcher.Dispatch(new GetUserMessagesHistoryQuery(userId));

            var messagesDTO = _mapper.Map<IEnumerable<GroupChat>>(messages);

            return Ok(messagesDTO);
        }

        [HttpGet]
        [Route("{chatId:guid}")]
        public async Task<IActionResult> GetGroupChatById([FromRoute]Guid chatId)
        {
            var query = new GetFullGroupChatQuery(chatId);

            var groupChat = await _handlersDispatcher.Dispatch(query);

            var groupChatDTO = _mapper.Map<GroupChat>(groupChat);

            return Ok(groupChatDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupChat([FromBody] CreateGroupChatRequest createGroupChatRequest)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return NotFound("Identity is null");
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User id is null");
            }

            var result = await _handlersDispatcher.Dispatch(new CreateGroupChatCommand(createGroupChatRequest.Name, userId));

            if (!result) return BadRequest("Something went wrong");

            return Ok();
        }

        [HttpDelete]
        [Route("{chatId:guid}")]
        public async Task<IActionResult> DeleteGroupChatById([FromRoute] Guid chatId)
        {
            var result = await _handlersDispatcher.Dispatch(new DeleteGroupChatCommand(Convert.ToString(chatId)));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Route("{chatId:guid}/editInfo")]
        public async Task<IActionResult> EditGroupChatInfoById([FromRoute] Guid chatId,
            [FromBody] EditGroupChatRequest editGroupChatRequest)
        {
            var result = await _handlersDispatcher.Dispatch(
                new EditGroupChatCommand(Convert.ToString(chatId), editGroupChatRequest.Name));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Route("{chatId:guid}/addUsers")]
        public async Task<IActionResult> AddUsersToGroupChat([FromRoute] Guid chatId,
            [FromBody] AddUsersToGroupRequest addUsersRequest)
        {
            var result = await _handlersDispatcher.Dispatch(
                new AddUsersToGroupChatCommand(Convert.ToString(chatId), addUsersRequest.Users));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("{chatId:guid}/removeUsers")]
        public async Task<IActionResult> RemoveUsersFromGroupChat([FromRoute] Guid chatId,
            [FromBody] RemoveUsersFromGroupRequest removeUsersRequest)
        {
            var result = await _handlersDispatcher.Dispatch(new RemoveUsersFromGroupChatCommand(Convert.ToString(chatId), removeUsersRequest.Users));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [Route("{chatId:guid}/send")]
        public async Task<IActionResult> SendMessageToGroupChat([FromRoute] Guid chatId,
            [FromBody] SendMessageToGroupChatRequest sendMessageRequest)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return NotFound("Identity is null");
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User id is null");
            }

            var result = await _handlersDispatcher.Dispatch(
                new SendMessageToGroupChatCommand(Convert.ToString(chatId), userId, sendMessageRequest.Text));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("{chatId:guid}/deleteMessage")]
        public async Task<IActionResult> DeleteMessageFromGroupChat([FromRoute] Guid chatId, [FromBody] DeleteMessageFromGroupChatRequest deleteMessageRequest)
        {
            var result = await _handlersDispatcher.Dispatch(
                new DeleteMessageFromGroupChatCommand(Convert.ToString(chatId), deleteMessageRequest.MessageId));

            if (!result) return BadRequest();

            return Ok();
        }
    }
}
