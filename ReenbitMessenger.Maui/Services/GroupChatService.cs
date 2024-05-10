using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.Maui.Clients;

namespace ReenbitMessenger.Maui.Services
{
    public class GroupChatService
    {
        private readonly IHttpClientWrapper _httpClient;
        private const string controllerPathBase = "https://rb-messenger.azurewebsites.net/groupchat/";

        public GroupChatService(IHttpClientWrapper httpClientWrapper)
        {
            _httpClient = httpClientWrapper;
        }

        public void Initialize()
        {
            _httpClient.Initialize();
        }

        public async Task<IEnumerable<GroupChatMessage>> GetMessagesHistory(GetMessagesHistoryRequest getMessagesRequest)
        {
            var response = await _httpClient
                .PostAsync<List<GroupChatMessage>, GetMessagesHistoryRequest>(controllerPathBase + "messagesHistory", getMessagesRequest);

            return response is null ? new List<GroupChatMessage>() : response;
        }

        public async Task<IEnumerable<GroupChat>> GetUserGroupChatsAsync(GetGroupChatsRequest getChatsRequest)
        {
            var response = await _httpClient
                .PostAsync<List<GroupChat>, GetGroupChatsRequest>(controllerPathBase + "userGroupChats", getChatsRequest);

            return response is null ? new List<GroupChat>() : response;
        }

        public async Task<bool> CreateGroupChatAsync(CreateGroupChatRequest createChatRequest)
        {
            return await _httpClient
                .PostAsync(controllerPathBase, createChatRequest);
        }

        public async Task<bool> JoinGroupChatAsync(string chatId)
        {
            return await _httpClient.GetAsync<bool>(controllerPathBase + chatId + "/join");
        }

        public async Task<bool> LeaveGroupChatAsync(string chatId)
        {
            return await _httpClient.DeleteAsync(controllerPathBase + chatId + "/leave");
        }

        public async Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistoryAsync()
        {
            var response = await _httpClient
                .GetAsync<List<GroupChatMessage>>(controllerPathBase + "messagesHistory");

            return response is null ? new List<GroupChatMessage>() : response;
        }
    }
}
