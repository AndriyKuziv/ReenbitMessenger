using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Maui.Clients
{
    public class HttpClientBase
    {
        protected static readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7051")
        };
        protected readonly ILocalStorageService _localStorage;

        public HttpClientBase(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected async Task<bool> HasToken()
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

        public async Task DeleteToken()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization is null)
            {
                return;
            }
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
