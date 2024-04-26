using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.AppServices.PrivateMessageServices.Queries;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using AutoMapper;

namespace ReenbitMessenger.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PrivateMessageController : Controller
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly IMapper _mapper;

        public PrivateMessageController(HandlersDispatcher handlersDispatcher, IMapper mapper)
        {
            _handlersDispatcher = handlersDispatcher;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> GetPrivateChat([FromBody]GetPrivateChatRequest getChatRequest)
        {
            var currUserId = await ControllerHelper.GetUserId(HttpContext);
            if (string.IsNullOrEmpty(currUserId))
            {
                return BadRequest("Cannot obtain user id from token.");
            }

            var chat = await _handlersDispatcher.Dispatch(new GetPrivateChatQuery(currUserId, getChatRequest.UserId));

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

            var result = await _handlersDispatcher.Dispatch(new SendPrivateMessageCommand(
                currUserId, sendMessageRequest.ReceiverId, sendMessageRequest.Text, sendMessageRequest.MessageToReplyId));

            if (!result)
            {
                return BadRequest("Error while sending a message.");
            }

            return Ok();
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

            var result = await _handlersDispatcher.Dispatch(new EditPrivateMessageCommand(
                editMessageRequest.MessageId, editMessageRequest.Text));

            if (!result)
            {
                return BadRequest("Error while editing a message.");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("message/{msgId:long}")]
        public async Task<IActionResult> DeletePrivateMessage([FromRoute] long msgId)
        {
            var result = await _handlersDispatcher.Dispatch(new DeletePrivateMessageCommand(msgId));

            if (!result)
            {
                return BadRequest("Error while deleting a message.");
            }

            return Ok();
        }
    }
}
