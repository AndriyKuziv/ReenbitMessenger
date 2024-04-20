using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol;
using ReenbitMessenger.DataAccess.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.DataAccess.AppServices.Queries.GroupChatQueries;
using ReenbitMessenger.DataAccess.Utils;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System.Security.Claims;

namespace ReenbitMessenger.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
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

        public async Task ConnectToGroupChat(string chatId)
        {
            var resChat = await _handlersDispatcher.Dispatch(new GetFullGroupChatQuery(new Guid(chatId)));

            if (resChat is null)
            {
                return;
            }

            var resChatDTO = _mapper.Map<GroupChat>(resChat);

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            await Clients.Caller.SendAsync("ReceiveFullGroupChat", resChatDTO);
        }

        public async Task CreateGroupChat(CreateGroupChatRequest createRequest)
        {
            var userId = await GetUserId();
            if (userId is null)
            {
                return;
            }

            var resChat = await _handlersDispatcher.Dispatch(new CreateGroupChatCommand(createRequest.Name, userId));

            if (resChat is null)
            {
                return;
            }

            var resChatDTO = _mapper.Map<GroupChat>(resChat);

            await Clients.Caller.SendAsync("ReceiveGroupChat", resChatDTO);
        }

        public async Task SendGroupChatMessage(string chatId, SendMessageToGroupChatRequest sendMessageRequest)
        {
            var userId = await GetUserId();

            if (userId is null)
            {
                return;
            }

            var resMessage = await _handlersDispatcher.Dispatch(new SendMessageToGroupChatCommand(new Guid(chatId), userId, sendMessageRequest.Text));

            if (resMessage is null)
            {
                return;
            }

            var resMessageDTO = _mapper.Map<GroupChatMessage>(resMessage);

            await Clients.Group(chatId).SendAsync("ReceiveMessage", resMessageDTO);
        }

        public async Task AddUsersToGroupChat(string chatId, AddUsersToGroupChatRequest addUsersRequest)
        {
            var resMembers = await _handlersDispatcher
                .Dispatch(new AddUsersToGroupChatCommand(new Guid(chatId), addUsersRequest.Users));

            if (resMembers is null)
            {
                return;
            }

            var resMembersDTO = _mapper.Map<IEnumerable<GroupChatMember>>(resMembers);

            await Clients.Group(chatId).SendAsync("ReceiveMember", resMembersDTO);
        }

        private async Task<string> GetUserId()
        {
            if (Context.User is null)
            {
                return null;
            }

            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity is null)
            {
                return null;
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
