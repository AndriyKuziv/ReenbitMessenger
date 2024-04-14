using Blazored.LocalStorage;
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
        private readonly ILocalStorageService _localStorage;

        public UserHttpClient(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(GetUsersRequest getUsersRequest)
        {
            if (!await HasToken()) return null;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _localStorage.GetItemAsync<string>("jwt"));

            string jsonRequestBody = JsonConvert.SerializeObject(getUsersRequest);
            HttpContent content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "users/usersList", content);

            if (!response.IsSuccessStatusCode) return null;

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<User>>(jsonResponse);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
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

        private async Task<bool> HasToken()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization is null)
            {
                var jwt = await _localStorage.GetItemAsync<string>("jwt");
                if (string.IsNullOrEmpty(jwt))
                {
                    return false;
                }

                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);
            }

            return true;
        }
    }
}
