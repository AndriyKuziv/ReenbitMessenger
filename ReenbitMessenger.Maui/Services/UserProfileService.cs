using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.Maui.Clients;

namespace ReenbitMessenger.Maui.Services
{
    public class UserProfileService
    {
        private readonly IHttpClientWrapper _httpClient;
        private const string controllerPathBase = "https://localhost:7051/users/";
        public UserProfileService(IHttpClientWrapper httpClientWrapper)
        {
            _httpClient = httpClientWrapper;
        }

        public async Task Initialize()
        {
            await _httpClient.Initialize();
        }

        public async Task<User> GetUserProfileAsync()
        {
            return await _httpClient.GetAsync<User>(controllerPathBase);
        }

        public async Task<User> EditUserInfoAsync(EditUserInfoRequest editUserInfoRequest)
        {
            return await _httpClient.PutAsync<User, EditUserInfoRequest>(controllerPathBase, editUserInfoRequest);
        }

        public async Task<bool> DeleteUserAsync()
        {
            return await _httpClient.DeleteAsync(controllerPathBase);
        }
    }
}
