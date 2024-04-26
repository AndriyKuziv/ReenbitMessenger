using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.AppServices.GroupChatServices.Queries;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
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
            var currUserId = await ControllerHelper.GetUserId(HttpContext);
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
            var currUserId = await ControllerHelper.GetUserId(HttpContext);
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
            var currUserId = await ControllerHelper.GetUserId(HttpContext);
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

            var chatResult = await _handlersDispatcher.Dispatch(command);

            if (chatResult is null) return BadRequest("Error during chat creation");

            var chatResultDTO = _mapper.Map<GroupChat>(chatResult);

            return Ok(chatResultDTO);
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

            var deleteSuccess = await _handlersDispatcher.Dispatch(command);

            if (!deleteSuccess) return BadRequest();

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

            var editSuccess = await _handlersDispatcher.Dispatch(command);

            if (!editSuccess) return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Route("{chatId:guid}/addUsers")]
        public async Task<IActionResult> AddUsersToGroupChat([FromRoute] Guid chatId,
            [FromBody] AddUsersToGroupChatRequest addUsersRequest)
        {
            var command = new AddUsersToGroupChatCommand(chatId, addUsersRequest.UsersIds);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var addResult = await _handlersDispatcher.Dispatch(command);

            if (addResult is null) return BadRequest("Error during adding of members");

            var addResultDTO = _mapper.Map<IEnumerable<GroupChatMember>>(addResult);

            return Ok(addResultDTO);
        }

        [HttpDelete]
        [Route("{chatId:guid}/removeUsers")]
        public async Task<IActionResult> RemoveUsersFromGroupChat([FromRoute] Guid chatId,
            [FromBody] RemoveUsersFromGroupChatRequest removeUsersRequest)
        {
            var command = new RemoveUsersFromGroupChatCommand(chatId, removeUsersRequest.UsersIds);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var removedMembers = await _handlersDispatcher.Dispatch(command);

            if (removedMembers is null) return BadRequest("Error during removing of members");

            return Ok();
        }

        [HttpGet]
        [Route("{chatId:guid}/join")]
        public async Task<IActionResult> JoinGroupChat([FromRoute] Guid chatId)
        {
            var userId = await ControllerHelper.GetUserId(HttpContext);
            var command = new AddUsersToGroupChatCommand(chatId, new List<string>() { userId });

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var addResult = await _handlersDispatcher.Dispatch(command);

            if (addResult is null) return BadRequest("Error during adding of members");

            var addResultDTO = _mapper.Map<IEnumerable<GroupChatMember>>(addResult);

            return Ok(addResultDTO);
        }

        [HttpDelete]
        [Route("{chatId:guid}/leave")]
        public async Task<IActionResult> LeaveGroupChat([FromRoute] Guid chatId)
        {
            var userId = await ControllerHelper.GetUserId(HttpContext);
            var command = new RemoveUsersFromGroupChatCommand(chatId, new List<string>() { userId });

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var removedMembers = await _handlersDispatcher.Dispatch(command);

            if (removedMembers is null) return BadRequest("Error during leaving from chat");

            return Ok();
        }

        [HttpPost]
        [Route("{chatId:guid}/send")]
        public async Task<IActionResult> SendMessageToGroupChat([FromRoute] Guid chatId,
            [FromBody] SendMessageToGroupChatRequest sendMessageRequest)
        {
            var currUserId = await ControllerHelper.GetUserId(HttpContext);
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
        public async Task<IActionResult> DeleteMessagesFromGroupChat([FromRoute] Guid chatId, [FromBody] DeleteMessageFromGroupChatRequest deleteMessageRequest)
        {
            var userId = await ControllerHelper.GetUserId(HttpContext);
            var command = new DeleteMessageFromGroupChatCommand(chatId, userId, deleteMessageRequest.MessageId);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var deletedMessage = await _handlersDispatcher.Dispatch(command);

            if (deletedMessage is null)
            {
                return BadRequest("Error during deleting the message");
            }

            var deletedMessageDTO = _mapper.Map<GroupChatMessage>(deletedMessage);

            return Ok(deletedMessageDTO);
        }
    }
}
