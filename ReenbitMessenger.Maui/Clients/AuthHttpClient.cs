using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ReenbitMessenger.Maui.Clients
{
    public class AuthHttpClient : IAuthHttpClient
    {
        private static readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7051")
        };

        public async Task<string> LogInAsync(LoginRequest loginRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        public async Task<bool> RegisterAsync(CreateUserRequest createUserRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/signup", createUserRequest);

            return response.IsSuccessStatusCode;
        }
    }
}
