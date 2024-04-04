using Java.Net;
using ReenbitMessenger.Library.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Maui.Clients
{
    public class UserHttpClient : IUserHttpClient
    {
        private readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:7051/")
        };

        public async Task<string> LoginAsync()
        {

        }

        public async Task<User?> GetUserAsync(Guid id)
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
