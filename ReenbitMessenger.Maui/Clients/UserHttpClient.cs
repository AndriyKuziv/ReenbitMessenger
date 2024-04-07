using ReenbitMessenger.Infrastructure.Models.DTO;
using System.Net.Http.Json;
using System.Text.Json;

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
            string jsonRequestBody = JsonSerializer.Serialize(getUsersRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            var result = _httpClient.GetAsync(_httpClient.BaseAddress + "User").Result;

            return null;
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
