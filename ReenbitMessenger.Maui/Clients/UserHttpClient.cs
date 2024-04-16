using Blazored.LocalStorage;
using Newtonsoft.Json;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System.Net.Http.Json;

namespace ReenbitMessenger.Maui.Clients
{
    public class UserHttpClient : HttpClientBase, IUserHttpClient
    {
        private const string controllerPathBase = "users/";

        public UserHttpClient(ILocalStorageService localStorage) : base(localStorage) { }

        public async Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest getUsersRequest)
        {
            if (!await HasToken()) return null;

            string jsonRequestBody = JsonConvert.SerializeObject(getUsersRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient
                .PostAsync(_httpClient.BaseAddress + controllerPathBase + "usersList", content);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<User>>(jsonResponse);
        }

        public async Task<User> GetUserAsync(string userId)
        {
            if (!await HasToken()) return null;

            HttpResponseMessage response = await _httpClient
                .GetAsync(_httpClient.BaseAddress + controllerPathBase + userId);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(jsonResponse);
        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> EditUserInfoAsync(string userId, EditUserInfoRequest editUserInfoRequest)
        {
            if (!await HasToken()) return null;

            string jsonRequestBody = JsonConvert.SerializeObject(editUserInfoRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient
                .PutAsync(_httpClient.BaseAddress + controllerPathBase + userId, content);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(jsonResponse);
        }
    }
}
