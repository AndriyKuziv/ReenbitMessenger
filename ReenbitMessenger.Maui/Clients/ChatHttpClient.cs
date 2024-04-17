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
            if (!await HasToken()) return null;

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GroupChat/userGroupChats");

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<GroupChat>>(jsonResponse);
        }

        public async Task<GroupChat> GetFullGroupChatAsync(string chatId)
        {
            if (!await HasToken()) return null;

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"GroupChat/{chatId}");

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<GroupChat>(jsonResponse);
        }

        public async Task<IEnumerable<GroupChatMessage>> GetUserGroupChatsMessagesHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendMessageToGroupChatAsync(string chatId, SendMessageToGroupChatRequest sendMessageRequest)
        {
            if (!await HasToken()) return false;

            string jsonRequestBody = JsonConvert.SerializeObject(sendMessageRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"GroupChat/{chatId}/send", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<PrivateMessage>> GetUserPrivateMessagesHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendPrivateMessageAsync(SendPrivateMessageRequest sendPrivateMessageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUsersToGroupChatAsync(string chatId, AddUsersToGroupRequest addUsersToChatRequest)
        {
            if (!await HasToken()) return false;

            string jsonRequestBody = JsonConvert.SerializeObject(addUsersToChatRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + $"GroupChat/{chatId}/addUsers", content);

            return response.IsSuccessStatusCode;
        }
    }
}
