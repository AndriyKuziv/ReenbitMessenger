using Blazored.LocalStorage;

namespace ReenbitMessenger.Maui.Clients
{
    public class HttpClientBase
    {
        protected static readonly HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7051")
        };
        protected readonly ILocalStorageService _localStorage;
        protected readonly string controllerPathBase;

        public HttpClientBase(ILocalStorageService localStorage, string controllerPathBase = "")
        {
            _localStorage = localStorage;
            this.controllerPathBase = controllerPathBase;
        }

        protected async Task<bool> HasToken()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization is null)
            {
                var jwt = await GetToken();
                if (string.IsNullOrEmpty(jwt))
                {
                    return false;
                }

                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);
            }

            return true;
        }

        public async Task<bool> SetToken()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization is null)
            {
                var jwt = await GetToken();
                if (string.IsNullOrEmpty(jwt))
                {
                    return false;
                }

                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);
            }

            return true;
        }

        protected async Task<string> GetToken()
        {
            return await _localStorage.GetItemAsync<string>("jwt");
        }

        public static async Task DeleteToken()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization is null)
            {
                return;
            }
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
