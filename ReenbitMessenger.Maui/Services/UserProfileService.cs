using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.Maui.Clients;
using System.Net.Http.Headers;

namespace ReenbitMessenger.Maui.Services
{
    public class UserProfileService
    {
        private readonly IHttpClientWrapper _httpClient;
        protected readonly ILocalStorageService _localStorage;
        private const string controllerPathBase = "https://localhost:7051/users/";
        public UserProfileService(IHttpClientWrapper httpClientWrapper,
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClientWrapper;
            _localStorage = localStorageService;
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

        public async Task<string> UpdateUserAvatarAsync(IBrowserFile imageFile)
        {
            if (imageFile is null) return null;

            using HttpClient cl = new HttpClient();

            string jwt = await _localStorage.GetItemAsync<string>("jwt");
            cl.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);

            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(imageFile.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);

            content.Add(streamContent, "Avatar", imageFile.Name);

            var response = await cl.PostAsync(controllerPathBase + "avatar", content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> DeleteUserAvatarAsync()
        {
            return await _httpClient.DeleteAsync(controllerPathBase + "avatar");
        }
    }
}
