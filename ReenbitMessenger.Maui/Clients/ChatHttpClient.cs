using Blazored.LocalStorage;
using Newtonsoft.Json;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Maui.Clients
{
    public class ChatHttpClient : HttpClientBase, IChatHttpClient
    {
        private const string groupChatControllerPathBase = "GroupChat/";
        private const string privateChatControllerPathBase = "privatemessage/";
        public ChatHttpClient(ILocalStorageService localStorage) : base(localStorage) { }

        public async Task<IEnumerable<GroupChat>> GetUserGroupChatsAsync()
        {
            if (!await HasToken()) return null;

            HttpResponseMessage response = await _httpClient
                .GetAsync(_httpClient.BaseAddress + groupChatControllerPathBase + "userGroupChats");

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<GroupChat>>(jsonResponse);
        }

        public async Task<GroupChat> GetFullGroupChatAsync(string chatId)
        {
            if (!await HasToken()) return null;

            HttpResponseMessage response = await _httpClient
                .GetAsync(_httpClient.BaseAddress + groupChatControllerPathBase + chatId);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<GroupChat>(jsonResponse);
        }
        public async Task<bool> CreateNewGroupChatAsync(CreateGroupChatRequest createChatRequest)
        {
            if (!await HasToken()) return false;

            string jsonRequestBody = JsonConvert.SerializeObject(createChatRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + groupChatControllerPathBase, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistoryAsync()
        {
            if (!await HasToken()) return null;

            HttpResponseMessage response = await _httpClient
                .GetAsync(_httpClient.BaseAddress + groupChatControllerPathBase + "messagesHistory");

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<GroupChatMessage>>(jsonResponse);
        }

        public async Task<bool> SendMessageToGroupChatAsync(string chatId, SendMessageToGroupChatRequest sendMessageRequest)
        {
            if (!await HasToken()) return false;

            string jsonRequestBody = JsonConvert.SerializeObject(sendMessageRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient
                .PostAsync(_httpClient.BaseAddress + groupChatControllerPathBase + $"{chatId}/send", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<PrivateMessage>> GetUserPrivateChat(string userId)
        {
            if (!await HasToken()) return null;

            string jsonRequestBody = JsonConvert.SerializeObject(new GetPrivateChatRequest { UserId = userId});
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient
                .PostAsync(_httpClient.BaseAddress + privateChatControllerPathBase + userId, content);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<PrivateMessage>>(jsonResponse);
        }

        public async Task<bool> SendPrivateMessageAsync(SendPrivateMessageRequest sendPrivateMessageRequest)
        {
            if (!await HasToken()) return false;

            string jsonRequestBody = JsonConvert.SerializeObject(sendPrivateMessageRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient
                .PostAsync(_httpClient.BaseAddress + privateChatControllerPathBase + "send", content);

            return response.IsSuccessStatusCode;
        }

    }
}
