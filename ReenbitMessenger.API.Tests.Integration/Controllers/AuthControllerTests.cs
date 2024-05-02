using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ReenbitMessenger.API.Tests.Integration.TestUtils;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ReenbitMessenger.API.Tests.Integration.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost:7051/auth/");

            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task LogIn_ValidCredentials_ReturnsSuccessStatusCode()
        {
            LoginRequest validRequest = new LoginRequest
            {
                Username = "testUser",
                Password = "Test0="
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/login", validRequest);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task LogIn_ValidCredentials_ReturnsContent()
        {
            LoginRequest validRequest = new LoginRequest
            {
                Username = "testUser",
                Password = "Test0="
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/login", validRequest);

            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task LogIn_InvalidCredentials_ReturnsBadRequestResult()
        {
            LoginRequest loginRequest = new LoginRequest {
                Username = "invalidUsername",
                Password = "invalidPassword"
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/login", loginRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        //[Fact]
        public async Task SignUp_ValidCredentials_ReturnsSuccessStatusCode()
        {
            CreateUserRequest validRequest = new CreateUserRequest
            {
                Username = "newTestUser",
                Email = "newTest@test.com",
                Password = "Test0="
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/signup", validRequest);

            response.EnsureSuccessStatusCode();
        }

        //[Fact]
        public async Task SignUp_InvalidCredentials_ReturnsBadRequestResult()
        {
            CreateUserRequest createUserRequest = new CreateUserRequest {
                Username = "newTestUser",
                Email = "invalidemail",
                Password = "Test0"
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/signup", createUserRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
