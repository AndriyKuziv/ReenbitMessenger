using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using ReenbitMessenger.API.Tests.Integration.TestUtils;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ReenbitMessenger.API.Tests.Integration.Controllers
{
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost:7051/users/");
            factory.Server.PreserveExecutionContext = true;

            _factory = factory;
            _httpClient = factory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        [Fact]
        public async Task GetSortedUsers_ValidRequest_ReturnsUserList()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            GetUsersRequest validRequest = new GetUsersRequest
            {
                Page = 0,
                NumberOfUsers = 20,
                ValueContains = "",
                OrderBy = "UserName",
                Ascending = true,
            };

            var response = await _httpClient.PostAsJsonAsync("usersList", validRequest);

            response.EnsureSuccessStatusCode();

            var usersList = JsonConvert.DeserializeObject<List<User>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(usersList);
            Assert.NotEmpty(usersList);
        }

        [Fact]
        public async Task GetUserById_ValidId_ReturnsUser()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid("9ce16ede-4614-46d4-a593-fbcfdc6c871c");

            var response = await _httpClient.GetAsync($"{userId}");

            response.EnsureSuccessStatusCode();

            var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUserById_InvalidId_ReturnsNotFoundResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid("9ce26ede-4614-46d4-a593-fbcfdc6c871c");

            var response = await _httpClient.GetAsync($"{userId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUserById_ValidId_ReturnsDeletedUser()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid("9ce16ede-4614-46d4-a593-fbcfdc6c871c");

            var response = await _httpClient.DeleteAsync($"{userId}");

            response.EnsureSuccessStatusCode();

            var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(user);
        }

        [Fact]
        public async Task DeleteUserById_InvalidId_ReturnsBadRequestResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid("9ce26ede-4614-46d4-a593-fbcfdc6c871c");

            var response = await _httpClient.DeleteAsync($"{userId}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task EditUserInfoById_ValidRequest_ReturnsEditedUser()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid("55d1a01f-6bb8-4be4-8091-a6ec47348afd");

            EditUserInfoRequest validRequest = new EditUserInfoRequest()
            {
                Email = "updated@gmail.com",
                Username = "updatedUsername"
            };

            var response = await _httpClient.PutAsJsonAsync($"{userId}", validRequest);

            response.EnsureSuccessStatusCode();

            var resultUser = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultUser);
            Assert.Equal(resultUser.UserName, validRequest.Username);
            Assert.Equal(resultUser.Email, validRequest.Email);
        }

        [Fact]
        public async Task EditUserInfoById_InvalidRequest_ReturnsBadRequestResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid("9ce26ede-4614-46d4-a593-fbcfdc6c871c");

            EditUserInfoRequest validRequest = new EditUserInfoRequest()
            {
                Email = "updated@gmail.com",
                Username = "updatedUsername"
            };

            var response = await _httpClient.PutAsJsonAsync($"{userId}", validRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task EditUserInfo_ValidRequest_ReturnsEditedUser()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditUserInfoRequest validRequest = new EditUserInfoRequest()
            {
                Email = "selfUpdated@gmail.com",
                Username = "selfUpdatedUsername"
            };

            var response = await _httpClient.PutAsJsonAsync("", validRequest);

            response.EnsureSuccessStatusCode();

            var resultUser = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultUser);
            Assert.Equal(resultUser.UserName, validRequest.Username);
            Assert.Equal(resultUser.Email, validRequest.Email);
        }

        [Fact]
        public async Task EditUserInfo_InvalidRequest_ReturnsBadRequestResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditUserInfoRequest validRequest = new EditUserInfoRequest()
            {
                Email = "test@gmail.com",
                Username = "selfUpdatedUsername"
            };

            var response = await _httpClient.PutAsJsonAsync("", validRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
