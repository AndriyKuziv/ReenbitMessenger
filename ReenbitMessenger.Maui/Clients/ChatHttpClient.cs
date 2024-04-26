using Blazored.LocalStorage;
using Newtonsoft.Json;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Collections.Generic;

namespace ReenbitMessenger.Maui.Clients
{
    public class ChatHttpClient : HttpClientBase, IChatHttpClient
    {
        public ChatHttpClient(ILocalStorageService localStorage) : base(localStorage, "groupchat/") { }

        public async Task<IEnumerable<GroupChatMessage>> GetMessagesHistory(GetMessagesHistoryRequest getMessagesRequest)
        {
            HttpResponseMessage response = await _httpClient
                .PostAsJsonAsync(_httpClient.BaseAddress + controllerPathBase + "messagesHistory", getMessagesRequest);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<GroupChatMessage>>(jsonResponse);

            return result is null ? new List<GroupChatMessage>() : result;
        }

        public async Task<IEnumerable<GroupChat>> GetUserGroupChatsAsync(GetGroupChatsRequest getChatsRequest)
        {
            HttpResponseMessage response = await _httpClient
                .PostAsJsonAsync(_httpClient.BaseAddress + controllerPathBase + "userGroupChats", getChatsRequest);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<GroupChat>>(jsonResponse);

            return result is null ? new List<GroupChat>() : result;
        }

        public async Task<bool> CreateGroupChatAsync(CreateGroupChatRequest createChatRequest)
        {
            HttpResponseMessage response = await _httpClient
                .PostAsJsonAsync(_httpClient.BaseAddress + controllerPathBase, createChatRequest);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> JoinGroupChatAsync(string chatId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + controllerPathBase + chatId + "/join");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LeaveGroupChatAsync(string chatId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + controllerPathBase +  chatId + "/leave");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistoryAsync()
        {
            HttpResponseMessage response = await _httpClient
                .GetAsync(_httpClient.BaseAddress + controllerPathBase + "messagesHistory");

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var resultMessages = JsonConvert.DeserializeObject<IEnumerable<GroupChatMessage>>(jsonResponse);

            return resultMessages is null ? new List<GroupChatMessage>() : resultMessages;
        }
    }
}
