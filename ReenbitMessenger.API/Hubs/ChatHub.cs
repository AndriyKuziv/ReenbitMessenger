using Microsoft.AspNetCore.SignalR;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.Infrastructure.Models.DTO;

namespace ReenbitMessenger.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly HandlersDispatcher _handlersDispatcher;
        public ChatHub(HandlersDispatcher handlersDispatcher)
        {
            _handlersDispatcher = handlersDispatcher;
        }

        public override async Task OnConnectedAsync()
        {
            //await SendGroupChatMessage("", new SendMessageToGroupChatRequest());

            await base.OnConnectedAsync();
        }

        public async Task SendGroupChatMessage(string chatId, string userId, SendMessageToGroupChatRequest sendMessageRequest)
        {
            var res = await _handlersDispatcher.Dispatch(new SendMessageToGroupChatCommand(new Guid(chatId), userId, sendMessageRequest.Text));

            if (!res) return;

            await Clients.All.SendAsync("ReceiveMessage", new GroupChatMessage() { SenderUser = new User() { 
                UserName = "signalUser"
                },
                Text = "signalRTest",
                SentTime = DateTime.Now,
            });
        }
    }
}
