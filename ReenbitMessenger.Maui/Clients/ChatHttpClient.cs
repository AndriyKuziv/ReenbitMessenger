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
        public ChatHttpClient(ILocalStorageService localStorage) : base(localStorage) { }

        public async Task<IEnumerable<GroupChat>> GetUserGroupChatsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GroupChat/userGroupChats");

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<GroupChat>>(jsonResponse);

            return result is null ? new List<GroupChat>() : result;
        }

        public async Task<GroupChat> GetFullGroupChatAsync(string chatId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"GroupChat/{chatId}");

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var resultGroupChat = JsonConvert.DeserializeObject<GroupChat>(jsonResponse);

            return resultGroupChat is null ? new GroupChat() : resultGroupChat;
        }

        public async Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistoryAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"GroupChat/messagesHistory");

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var resultMessages = JsonConvert.DeserializeObject<IEnumerable<GroupChatMessage>>(jsonResponse);

            return resultMessages is null ? new List<GroupChatMessage>() : resultMessages;
        }

        public async Task<bool> SendMessageToGroupChatAsync(string chatId, SendMessageToGroupChatRequest sendMessageRequest)
        {
            string jsonRequestBody = JsonConvert.SerializeObject(sendMessageRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"GroupChat/{chatId}/send", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddUsersToGroupChatAsync(string chatId, AddUsersToGroupChatRequest addUsersToChatRequest)
        {
            string jsonRequestBody = JsonConvert.SerializeObject(addUsersToChatRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + $"GroupChat/{chatId}/addUsers", content);

            return response.IsSuccessStatusCode;
        }
    }
}
