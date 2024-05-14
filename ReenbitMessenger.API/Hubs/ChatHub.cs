using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.AppServices.GroupChatServices.Queries;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using AutoMapper;

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
            var resChat = await _handlersDispatcher.Dispatch(new GetGroupChatInfoQuery(new Guid(chatId)));

            if (resChat is null)
            {
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task DisconnectFromGroupChat(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task GetGroupChatInfo(string chatId)
        {
            var resChat = await _handlersDispatcher.Dispatch(new GetGroupChatInfoQuery(new Guid(chatId)));

            if (resChat is null)
            {
                return;
            }

            var resChatDTO = _mapper.Map<GroupChat>(resChat);

            await Clients.Caller.SendAsync("ReceiveGroupChatInfo", resChatDTO);
        }

        public async Task GetGroupChatMessages(string chatId,
            int page = 0, int numberOfMessages = 20, string messageContains = "", bool ascending = true)
        {
            var resMessages = await _handlersDispatcher
                .Dispatch(new GetGroupChatMessagesQuery(new Guid(chatId), numberOfMessages, messageContains, page, ascending));

            if (resMessages is null)
            {
                return;
            }

            var resMessagesDTO = _mapper.Map<IEnumerable<GroupChatMessage>>(resMessages);

            await Clients.Caller.SendAsync("ReceiveGroupChatMessages", resMessagesDTO);
        }

        public async Task CreateGroupChat(CreateGroupChatRequest createRequest)
        {
            var userId = await HubHelper.GetUserIdAsync(Context);
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
            var userId = await HubHelper.GetUserIdAsync(Context);

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

        public async Task DeleteGroupChatMessage(string chatId, DeleteMessageFromGroupChatRequest deleteMessageRequest)
        {
            var userId = await HubHelper.GetUserIdAsync(Context);

            if (userId is null)
            {
                return;
            }

            var removedMessage = await _handlersDispatcher.Dispatch(new DeleteMessageFromGroupChatCommand(new Guid(chatId), userId, deleteMessageRequest.MessageId));

            if (removedMessage is null)
            {
                return;
            }

            var removedMessageDTO = _mapper.Map<GroupChatMessage>(removedMessage);

            await Clients.Group(chatId).SendAsync("DeleteMessage", removedMessageDTO);
        }

        public async Task AddUsersToGroupChat(string chatId, AddUsersToGroupChatRequest addUsersRequest)
        {
            var addedMembers = await _handlersDispatcher
                .Dispatch(new AddUsersToGroupChatCommand(new Guid(chatId), addUsersRequest.UsersIds));

            if (addedMembers is null)
            {
                return;
            }

            var addedMembersDTO = _mapper.Map<IEnumerable<GroupChatMember>>(addedMembers);

            await Clients.Group(chatId).SendAsync("ReceiveMembers", addedMembersDTO);
        }

        public async Task RemoveUsersFromGroupChat(string chatId, RemoveUsersFromGroupChatRequest removeUsersRequest)
        {
            var removedMembersIds = await _handlersDispatcher
                .Dispatch(new RemoveUsersFromGroupChatCommand(new Guid(chatId), removeUsersRequest.UsersIds));

            if (removedMembersIds is null)
            {
                return;
            }

            await Clients.Group(chatId).SendAsync("RemoveMembers", removedMembersIds);
        }
    }
}
