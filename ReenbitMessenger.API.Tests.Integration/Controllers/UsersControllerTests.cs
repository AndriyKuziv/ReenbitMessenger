using Microsoft.AspNetCore.Identity;
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
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
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

            AddTestData();
        }

        [Fact]
        public async Task GetSortedUsers_ValidRequest_ReturnsUserList()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

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
            Assert.Equal(usersList.Count, testUsers.Count);
        }

        [Fact]
        public async Task GetUserById_ValidId_ReturnsUser()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = testUsers[1].Id;

            var response = await _httpClient.GetAsync($"{userId}");

            response.EnsureSuccessStatusCode();

            var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUserById_InvalidId_ReturnsBadRequestResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid("9ce26ede-4614-46d4-a593-fbcfdc6c871c");

            var response = await _httpClient.GetAsync($"{userId}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUserById_ValidId_ReturnsSuccessStatusCode()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid(testUsers[1].Id);

            var response = await _httpClient.DeleteAsync($"{userId}");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteUserById_InvalidId_ReturnsBadRequestResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = new Guid("9ce26ede-4614-46d4-a593-fbcfdc6c871c");

            var response = await _httpClient.DeleteAsync($"{userId}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task EditUserInfoById_ValidRequest_ReturnsEditedUser()
        {
            var testUser = testUsers[1];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = testUser.Id;

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
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

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
            var testUser = testUsers[1];
            var token = TestsHelper.GetValidToken(testUser);

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
            var testUser = testUsers[1];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditUserInfoRequest validRequest = new EditUserInfoRequest()
            {
                Email = "test@gmail.com",
                Username = ""
            };

            var response = await _httpClient.PutAsJsonAsync("", validRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private void AddTestData()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();

            foreach (var user in testUsers)
            {
                if (!dbContext.Users.Contains(user))
                {
                    dbContext.Users.Add(user);
                }
            }

            dbContext.SaveChanges();
        }

        private void RemoveTestData()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();

            foreach( var user in testUsers)
            {
                if (dbContext.Users.Contains(user))
                {
                    dbContext.Users.Remove(user);
                }
            }
            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if(_httpClient != null)
            {
                _httpClient.Dispose();
            }
            RemoveTestData();
        }

        private List<IdentityUser> testUsers = new List<IdentityUser>()
        {
            new IdentityUser { Id = "00a11324-0a61-4893-b79f-54ba531ec2b8", UserName = "testUser1", Email = "test1@gmail.com" },
            new IdentityUser { Id = "67b53758-87aa-4f36-81fc-667c3ff16ca0", UserName = "testUser2", Email = "test2@gmail.com" },
            new IdentityUser { Id = "a0146c3a-3aa3-4bea-9917-58a7bcd3e35a", UserName = "testUser2", Email = "test1@gmail.com" },
            new IdentityUser { Id = "28d7154a-4eba-4a8f-8086-755994e9062b", UserName = "testUser2", Email = "test2@gmail.com" },
        };
    }
}
