using ReenbitMessenger.Infrastructure.Models.DTO;
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
            string jsonRequestBody = JsonSerializer.Serialize(loginRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();

                Console.WriteLine("JWT Token: " + jsonResponse);
                return jsonResponse;
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }

            return null;
        }


        public async Task<bool> RegisterAsync(CreateUserRequest createUserRequest)
        {
            string jsonRequestBody = JsonSerializer.Serialize(createUserRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("auth/register", content);

            return response.IsSuccessStatusCode;
        }
    }
}
