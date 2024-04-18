using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        private readonly IMapper _mapper;

        public ChatHub(HandlersDispatcher handlersDispatcher, IMapper mapper)
        {
            _handlersDispatcher = handlersDispatcher;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task SendGroupChatMessage(string chatId, string userId, SendMessageToGroupChatRequest sendMessageRequest)
        {
            var resMessage = await _handlersDispatcher.Dispatch(new SendMessageToGroupChatCommand(new Guid(chatId), userId, sendMessageRequest.Text));

            if (resMessage is null) return;

            var resMessageDTO = _mapper.Map<GroupChatMessage>(resMessage);

            await Clients.All.SendAsync("ReceiveMessage", resMessageDTO);
        }
    }
}
