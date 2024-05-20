using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.Maui.Clients;

namespace ReenbitMessenger.Maui.Services
{
    public class UserService
    {
        private readonly IHttpClientWrapper _httpClient;
        private const string controllerPathBase = "https://localhost:7051/users/";
        public UserService(IHttpClientWrapper httpClientWrapper)
        {
            _httpClient = httpClientWrapper;
        }

        public async Task Initialize()
        {
            await _httpClient.Initialize();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest getUsersRequest)
        {
            var response = await _httpClient
                .PostAsync<List<User>, GetUsersRequest>(controllerPathBase + "usersList", getUsersRequest);

            return response is null ? new List<User>() : response;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await _httpClient.GetAsync<User>(controllerPathBase + userId);
        }

        public async Task<User> EditUserInfoByIdAsync(string userId, EditUserInfoRequest editUserInfoRequest)
        {
            return await _httpClient.PutAsync<User, EditUserInfoRequest>(controllerPathBase + userId, editUserInfoRequest);
        }

        public async Task<bool> DeleteUserByIdAsync(string userId)
        {
            return await _httpClient.DeleteAsync(controllerPathBase + userId);
        }
    }
}
