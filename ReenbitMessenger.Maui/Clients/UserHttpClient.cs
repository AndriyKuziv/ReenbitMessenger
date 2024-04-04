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

        public async Task<string> LogInAsync(string email, string password)
        {
            var requestBody = new
            {
                email = email,
                password = password
            };
            string jsonRequestBody = JsonSerializer.Serialize(requestBody);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("login", content);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();

                var tokenResponse = JsonSerializer.Deserialize<Token>(jsonResponse);

                Console.WriteLine("JWT Token: " + tokenResponse.accessToken);
                return tokenResponse.accessToken;
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }

            return null;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<User>($"{id}");
        }

        public async Task<string> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
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
