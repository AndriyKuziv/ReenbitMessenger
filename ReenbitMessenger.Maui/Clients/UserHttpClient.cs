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
            string jsonRequestBody = JsonConvert.SerializeObject(getUsersRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "users/usersList", content);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

            return result is null ? new List<User>() : result;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<User>($"{userId}");
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
