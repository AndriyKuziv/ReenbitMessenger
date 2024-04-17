using Blazored.LocalStorage;
using Newtonsoft.Json;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System.Net.Http.Json;

namespace ReenbitMessenger.Maui.Clients
{
    public class UserHttpClient : HttpClientBase, IUserHttpClient
    {
        public UserHttpClient(ILocalStorageService localStorage) : base(localStorage) { }

        public async Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest getUsersRequest)
        {
            if (!await HasToken()) return null;

            string jsonRequestBody = JsonConvert.SerializeObject(getUsersRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "users/usersList", content);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<User>>(jsonResponse);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            if (!await HasToken()) return null;

            return await _httpClient.GetFromJsonAsync<User>($"{id}");
        }

        public async Task<string> DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> EditUserInfoAsync(Guid id, EditUserInfoRequest editUserInfoRequest)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> EditUserInfoAsync(string userId, EditUserInfoRequest editUserInfoRequest)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
