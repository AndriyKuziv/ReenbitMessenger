using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace ReenbitMessenger.Maui.Clients
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient = new HttpClient();
        protected readonly ILocalStorageService _localStorage;

        public HttpClientWrapper(ILocalStorageService localStorageService)
        {
            _localStorage = localStorageService;
        }

        public void Initialize()
        {
            new Action(async () => await SetToken())();
        }

        public async Task<TResponse?> GetAsync<TResponse>(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(responseBody);
        }

        public async Task<TResponse?> PostAsync<TResponse, TRequest>(string url, TRequest request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(responseBody);
        }

        public async Task<bool> PostAsync<TRequest>(string url, TRequest request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, request);

            return response.IsSuccessStatusCode;
        }

        public async Task<TResponse?> PutAsync<TResponse, TRequest>(string url, TRequest request)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(responseBody);
        }

        public async Task<TResponse?> DeleteAsync<TResponse>(string url)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(responseBody);
        }

        public async Task<bool> DeleteAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);

            return response.IsSuccessStatusCode;
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
