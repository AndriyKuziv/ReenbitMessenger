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
        private readonly IValidatorsHandler _validatorsHandler;

        public GroupChatController(HandlersDispatcher handlersDispatcher, 
            IMapper mapper, IValidatorsHandler validatorsHandler)
        {
            _handlersDispatcher = handlersDispatcher;
            _mapper = mapper;
            _validatorsHandler = validatorsHandler;
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

            var command = new CreateGroupChatCommand(createGroupChatRequest.Name, currUserId);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            // TODO: remake command to return created chat
            var createResult = await _handlersDispatcher.Dispatch(command);

            if (!createResult) return BadRequest("Something went wrong");

            return Ok();
        }

        [HttpDelete]
        [Route("{chatId:guid}")]
        public async Task<IActionResult> DeleteGroupChatById([FromRoute] Guid chatId)
        {
            var command = new DeleteGroupChatCommand(chatId);

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
        [Route("{chatId:guid}/editInfo")]
        public async Task<IActionResult> EditGroupChatInfoById([FromRoute] Guid chatId,
            [FromBody] EditGroupChatRequest editGroupChatRequest)
        {
            var command = new EditGroupChatCommand(chatId, editGroupChatRequest.Name);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var editResult = await _handlersDispatcher.Dispatch(command);

            if (!editResult) return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Route("{chatId:guid}/addUsers")]
        public async Task<IActionResult> AddUsersToGroupChat([FromRoute] Guid chatId,
            [FromBody] AddUsersToGroupRequest addUsersRequest)
        {
            var command = new AddUsersToGroupChatCommand(chatId, addUsersRequest.Users);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var addResult = await _handlersDispatcher.Dispatch(command);

            if (!addResult) return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Route("{chatId:guid}/removeUsers")]
        public async Task<IActionResult> RemoveUsersFromGroupChat([FromRoute] Guid chatId,
            [FromBody] RemoveUsersFromGroupRequest removeUsersRequest)
        {
            var command = new RemoveUsersFromGroupChatCommand(chatId, removeUsersRequest.Users);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var removeResult = await _handlersDispatcher.Dispatch(command);

            if (!removeResult) return BadRequest();

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

            var command = new SendMessageToGroupChatCommand(chatId, currUserId, sendMessageRequest.Text);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var groupChatMessage = await _handlersDispatcher.Dispatch(command);

            if (groupChatMessage is null) return BadRequest();

            var groupChatMessageDTO = _mapper.Map<GroupChatMessage>(groupChatMessage);

            return Ok(groupChatMessageDTO);
        }

        [HttpDelete]
        [Route("{chatId:guid}/deleteMessage")]
        public async Task<IActionResult> DeleteMessagesFromGroupChat([FromRoute] Guid chatId, [FromBody] DeleteMessagesFromGroupChatRequest deleteMessageRequest)
        {
            var command = new DeleteMessagesFromGroupChatCommand(chatId, deleteMessageRequest.MessagesIds);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var deleteResult = await _handlersDispatcher.Dispatch(command);

            if (!deleteResult) return BadRequest();

            return Ok();
        }

        // Private methods
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
