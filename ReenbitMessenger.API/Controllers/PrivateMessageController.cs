using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.AppServices.PrivateMessageServices.Queries;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using AutoMapper;
using Microsoft.CodeAnalysis.Emit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReenbitMessenger.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PrivateMessageController : Controller
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly IMapper _mapper;
        private readonly IValidatorsHandler _validatorsHandler;

        public PrivateMessageController(HandlersDispatcher handlersDispatcher, IMapper mapper,
            IValidatorsHandler validatorsHandler)
        {
            _handlersDispatcher = handlersDispatcher;
            _mapper = mapper;
            _validatorsHandler = validatorsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> GetPrivateChat([FromBody]GetPrivateChatRequest getChatRequest)
        {
            var currUserId = await ControllerHelper.GetUserId(HttpContext);
            if (string.IsNullOrEmpty(currUserId))
            {
                return BadRequest("Cannot obtain user id from token.");
            }

            var command = new GetPrivateChatQuery(currUserId, getChatRequest.UserId);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var chat = await _handlersDispatcher.Dispatch(command);

            if(chat is null)
            {
                return NotFound("Chat is null.");
            }

            var chatDTO = _mapper.Map<IEnumerable<PrivateMessage>>(chat);

            return Ok(chatDTO);
        }

        [HttpGet]
        [Route("message/{msgId:long}")]
        public async Task<IActionResult> GetPrivateMessage([FromRoute] long msgId)
        {
            var message = await _handlersDispatcher.Dispatch(new GetPrivateMessageQuery(msgId));

            if (message is null)
            {
                return NotFound("Message is null.");
            }

            var messageDTO = _mapper.Map<PrivateMessage>(message);

            return Ok(messageDTO);
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendPrivateMessage([FromBody] SendPrivateMessageRequest sendMessageRequest)
        {
            var currUserId = await ControllerHelper.GetUserId(HttpContext);
            if (string.IsNullOrEmpty(currUserId))
            {
                return BadRequest("Cannot obtain user id from token.");
            }

            var command = new SendPrivateMessageCommand(
                currUserId, sendMessageRequest.ReceiverId, sendMessageRequest.Text, sendMessageRequest.MessageToReplyId);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var sentMessage = await _handlersDispatcher.Dispatch(command);

            if (sentMessage is null)
            {
                return BadRequest("Error while sending a message.");
            }

            var sentMessageDTO = _mapper.Map<PrivateMessage>(sentMessage);

            return Ok(sentMessageDTO);
        }

        [HttpPut]
        [Route("message")]
        public async Task<IActionResult> EditPrivateMessage([FromBody] EditPrivateMessageRequest editMessageRequest)
        {
            var currUserId = await ControllerHelper.GetUserId(HttpContext);
            if (string.IsNullOrEmpty(currUserId))
            {
                return BadRequest("Cannot obtain user id from token.");
            }

            var command = new EditPrivateMessageCommand(
                editMessageRequest.MessageId, editMessageRequest.Text);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var editedMessage = await _handlersDispatcher.Dispatch(command);

            if (editedMessage is null)
            {
                return BadRequest("Error while editing a message.");
            }

            var editedMessageDTO = _mapper.Map<PrivateMessage>(editedMessage);

            return Ok(editedMessageDTO);
        }

        [HttpDelete]
        [Route("message/{msgId:long}")]
        public async Task<IActionResult> DeletePrivateMessage([FromRoute] long msgId)
        {
            var command = new DeletePrivateMessageCommand(msgId);

            var result = await _validatorsHandler.ValidateAsync(command);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var deletedMessage = await _handlersDispatcher.Dispatch(command);

            if (deletedMessage is null)
            {
                return BadRequest("Error while deleting a message.");
            }

            var deletedMessageDTO = _mapper.Map<PrivateMessage>(deletedMessage);

            return Ok(deletedMessageDTO);
        }
    }
}
