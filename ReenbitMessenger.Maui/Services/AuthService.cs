using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.Maui.Clients;

namespace ReenbitMessenger.Maui.Services
{
    public class AuthService
    {
        private readonly IHttpClientWrapper _httpClient;
        private const string controllerPathBase = "https://rb-messenger.azurewebsites.net/auth/";

        public AuthService(IHttpClientWrapper httpClientWrapper)
        {
            _httpClient = httpClientWrapper;
        }

        public async Task<string> LogInAsync(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsync<AuthToken, LoginRequest>(controllerPathBase + "login", loginRequest);
            return response.Token;
        }

        public async Task<bool> RegisterAsync(CreateUserRequest createUserRequest)
        {
            return await _httpClient.PostAsync(controllerPathBase + "signup", createUserRequest);
        }
    }
}
