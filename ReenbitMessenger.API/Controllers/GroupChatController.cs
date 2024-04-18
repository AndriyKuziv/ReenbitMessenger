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
            var currUserId = await GetUserId();
            if (string.IsNullOrEmpty(currUserId))
            {
                return BadRequest("Cannot obtain user id from token");
            }

            var groupChats = await _handlersDispatcher.Dispatch(new GetUserGroupChatsQuery(currUserId));

            var groupChatsDTO = _mapper.Map<IEnumerable<GroupChat>>(groupChats);

            return Ok(groupChatsDTO);
        }

        [HttpGet]
        [Route("messagesHistory")]
        public async Task<IActionResult> GetUserMessagesHistory()
        {
            var currUserId = await GetUserId();
            if (string.IsNullOrEmpty(currUserId))
            {
                return BadRequest("Cannot obtain user id from token");
            }

            var messages = await _handlersDispatcher.Dispatch(new GetUserMessagesHistoryQuery(currUserId));

            var messagesDTO = _mapper.Map<IEnumerable<GroupChatMessage>>(messages);

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
            var currUserId = await GetUserId();
            if (string.IsNullOrEmpty(currUserId))
            {
                return BadRequest("Cannot obtain user id from token");
            }

            var result = await _handlersDispatcher.Dispatch(new CreateGroupChatCommand(createGroupChatRequest.Name, currUserId));

            if (!result) return BadRequest("Something went wrong");

            return Ok();
        }

        [HttpDelete]
        [Route("{chatId:guid}")]
        public async Task<IActionResult> DeleteGroupChatById([FromRoute] Guid chatId)
        {
            var result = await _handlersDispatcher.Dispatch(new DeleteGroupChatCommand(chatId));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Route("{chatId:guid}/editInfo")]
        public async Task<IActionResult> EditGroupChatInfoById([FromRoute] Guid chatId,
            [FromBody] EditGroupChatRequest editGroupChatRequest)
        {
            var result = await _handlersDispatcher.Dispatch(
                new EditGroupChatCommand(chatId, editGroupChatRequest.Name));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Route("{chatId:guid}/addUsers")]
        public async Task<IActionResult> AddUsersToGroupChat([FromRoute] Guid chatId,
            [FromBody] AddUsersToGroupRequest addUsersRequest)
        {
            var result = await _handlersDispatcher.Dispatch(
                new AddUsersToGroupChatCommand(chatId, addUsersRequest.Users));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("{chatId:guid}/removeUsers")]
        public async Task<IActionResult> RemoveUsersFromGroupChat([FromRoute] Guid chatId,
            [FromBody] RemoveUsersFromGroupRequest removeUsersRequest)
        {
            var result = await _handlersDispatcher.Dispatch(new RemoveUsersFromGroupChatCommand(chatId, removeUsersRequest.Users));

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [Route("{chatId:guid}/send")]
        public async Task<IActionResult> SendMessageToGroupChat([FromRoute] Guid chatId,
            [FromBody] SendMessageToGroupChatRequest sendMessageRequest)
        {
            var currUserId = await GetUserId();
            if (string.IsNullOrEmpty(currUserId))
            {
                return BadRequest("Cannot obtain user id from token");
            }

            var groupChatMessage = await _handlersDispatcher.Dispatch(
                new SendMessageToGroupChatCommand(chatId, currUserId, sendMessageRequest.Text));

            if (groupChatMessage is null) return BadRequest();

            var groupChatMessageDTO = _mapper.Map<GroupChatMessage>(groupChatMessage);

            return Ok(groupChatMessageDTO);
        }

        [HttpDelete]
        [Route("{chatId:guid}/deleteMessage")]
        public async Task<IActionResult> DeleteMessageFromGroupChat([FromRoute] Guid chatId, [FromBody] DeleteMessageFromGroupChatRequest deleteMessageRequest)
        {
            var result = await _handlersDispatcher.Dispatch(
                new DeleteMessageFromGroupChatCommand(chatId, deleteMessageRequest.MessageId));

            if (!result) return BadRequest();

            return Ok();
        }

        private async Task<string> GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
