using Blazored.LocalStorage;
using Newtonsoft.Json;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Net.Http;
using System.Net.Http.Json;

namespace ReenbitMessenger.Maui.Clients
{
    public class UserHttpClient : HttpClientBase, IUserHttpClient
    {
        public UserHttpClient(ILocalStorageService localStorage) : base(localStorage, "users/") { }

        public async Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest getUsersRequest)
        {
            HttpResponseMessage response = await _httpClient
                .PostAsJsonAsync(_httpClient.BaseAddress + controllerPathBase + "usersList", getUsersRequest);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

            return result is null ? new List<User>() : result;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<User>($"{userId}");
        }

        public async Task<bool> EditUserInfoAsync(EditUserInfoRequest editUserInfoRequest)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + controllerPathBase, editUserInfoRequest);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync()
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + controllerPathBase);

            return response.IsSuccessStatusCode;
        }
    }
}
