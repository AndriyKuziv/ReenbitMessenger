using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ReenbitMessenger.API.Tests.Integration.TestUtils;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ReenbitMessenger.API.Tests.Integration.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
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
        public async Task LogIn_ValidCredentials_ReturnsToken()
        {
            await AddTestDataAsync();

            LoginRequest validRequest = new LoginRequest
            {
                Username = "testUser",
                Password = "Test0="
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/login", validRequest);

            response.EnsureSuccessStatusCode();

            var tokenObject = JsonConvert.DeserializeObject<AuthToken>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(tokenObject);
            Assert.NotNull(tokenObject.Token);
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

        [Fact]
        public async Task SignUp_ValidCredentials_ReturnsSuccessStatusCode()
        {
            CreateUserRequest validRequest = new CreateUserRequest
            {
                Username = signUpUser.UserName,
                Email = signUpUser.Email,
                Password = "Test0="
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/signup", validRequest);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
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

        private async Task AddTestDataAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();

            if (dbContext.Users.Contains(testUser))
            {
                return;
            }

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var result = await userManager.CreateAsync(testUser, "Test0=");
            if (result.Succeeded)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task ClearTestDataAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();

            if (dbContext.Users.Contains(testUser))
            {
                dbContext.Users.Remove(testUser);
            }

            var signedUpUser = dbContext.Users
                .FirstOrDefault(usr => usr.UserName == signUpUser.UserName && usr.Email == signUpUser.Email);
            if (signedUpUser != null)
            {
                dbContext.Users.Remove(signedUpUser);
            }

            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }

            ClearTestDataAsync().GetAwaiter().GetResult();
        }

        // Test data
        private IdentityUser testUser = new IdentityUser()
        {
            UserName = "testUser",
            Email = "test@test.com",
        };

        private IdentityUser signUpUser = new IdentityUser()
        {
            UserName = "newTestUser",
            Email = "newTest@test.com"
        };
    }
}
