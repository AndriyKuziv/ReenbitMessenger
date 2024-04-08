using Newtonsoft.Json;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System.Net.Http.Json;

namespace ReenbitMessenger.Maui.Clients
{
    public class UserHttpClient : IUserHttpClient
    {
        private static readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7051")
        };

        public async Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest getUsersRequest)
        {
            string jsonRequestBody = JsonConvert.SerializeObject(getUsersRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "users/usersList", content);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

            return users;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<User>($"{id}");
        }

        public Task<string> DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User?> EditUserInfoAsync(Guid id, EditUserInfoRequest editUserInfoRequest)
        {
            throw new NotImplementedException();
        }
    }
}
