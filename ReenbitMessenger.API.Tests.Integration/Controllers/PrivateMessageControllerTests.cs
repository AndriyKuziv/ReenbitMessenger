using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ReenbitMessenger.API.Tests.Integration.TestUtils;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Net.Http.Headers;

namespace ReenbitMessenger.API.Tests.Integration.Controllers
{
    public class PrivateMessageControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public PrivateMessageControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost:7051/privateMessage/");

            _factory = factory;
            _httpClient = factory.CreateClient();

            AddTestDataAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GetPrivateChat_ValidRequest_ReturnsPrivateMessagesList()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var secondTestUser = testUsers[1];
            GetPrivateChatRequest validRequest = new GetPrivateChatRequest
            {
                UserId = secondTestUser.Id
            };

            var expectedMessages = testMessages
                .Where(pm => (pm.SenderUserId == testUser.Id && pm.ReceiverUserId == secondTestUser.Id) ||
                            (pm.SenderUserId == secondTestUser.Id && pm.ReceiverUserId == testUser.Id));

            // Act
            var response = await _httpClient.PostAsJsonAsync("", validRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var resultMessages = JsonConvert
                .DeserializeObject<IEnumerable<PrivateMessage>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultMessages);
            Assert.Equal(expectedMessages.Count(), resultMessages.Count());
        }

        [Fact]
        public async Task SendPrivateMessage_ValidRequest_ReturnsSentPrivateMessage()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var secondTestUser = testUsers[1];
            SendPrivateMessageRequest validRequest = new SendPrivateMessageRequest
            {
                Text = "newText",
                ReceiverId = secondTestUser.Id
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("send", validRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var resultMessage = JsonConvert
                .DeserializeObject<PrivateMessage>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultMessage);
            Assert.Equal(validRequest.ReceiverId, resultMessage.ReceiverUserId);
            Assert.Equal(validRequest.Text, resultMessage.Text);
            Assert.Equal(validRequest.MessageToReplyId, resultMessage.MessageToReplyId);
        }

        [Fact]
        public async Task EditPrivateMessage_ValidRequest_ReturnsEditedPrivateMessage()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var secondTestUser = testUsers[1];
            GetPrivateChatRequest getChatRequest = new GetPrivateChatRequest
            {
                UserId = secondTestUser.Id
            };
            var privateChatResponse = await _httpClient.PostAsJsonAsync("", getChatRequest);

            privateChatResponse.EnsureSuccessStatusCode();

            var messages = JsonConvert
                .DeserializeObject<List<PrivateMessage>>(await privateChatResponse.Content.ReadAsStringAsync());

            var originalMessage = messages[1];

            EditPrivateMessageRequest validRequest = new EditPrivateMessageRequest
            {
                MessageId = originalMessage.Id,
                Text = "updatedText"
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync("message", validRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var resultMessage = JsonConvert
                .DeserializeObject<PrivateMessage>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultMessage);
            Assert.Equal(originalMessage.Id, resultMessage.Id);
            Assert.Equal(validRequest.Text, resultMessage.Text);
        }

        [Fact]
        public async Task DeletePrivateMessage_ValidRequest_ReturnsDeletedPrivateMessage()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var secondTestUser = testUsers[1];
            GetPrivateChatRequest getChatRequest = new GetPrivateChatRequest
            {
                UserId = secondTestUser.Id
            };
            var privateChatResponse = await _httpClient.PostAsJsonAsync("", getChatRequest);

            privateChatResponse.EnsureSuccessStatusCode();

            var messages = JsonConvert
                .DeserializeObject<List<PrivateMessage>>(await privateChatResponse.Content.ReadAsStringAsync());

            var originalMessage = messages[1];

            // Act
            var response = await _httpClient.DeleteAsync($"message/{originalMessage.Id}");

            // Assert
            response.EnsureSuccessStatusCode();

            var resultMessage = JsonConvert
                .DeserializeObject<PrivateMessage>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultMessage);
            Assert.Equal(originalMessage.Id, resultMessage.Id);
        }

        private async Task AddTestDataAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();

            foreach (var user in testUsers)
            {
                if (!dbContext.Users.Contains(user))
                {
                    await dbContext.Users.AddAsync(user);
                }
            }

            foreach (var message in testMessages)
            {
                if (!dbContext.PrivateMessage.Contains(message))
                {
                    await dbContext.PrivateMessage.AddAsync(message);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task ClearTestDataAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();

            foreach (var pm in dbContext.PrivateMessage)
            {
                dbContext.PrivateMessage.Remove(pm);
            }

            foreach (var user in testUsers)
            {
                if (dbContext.Users.Contains(user))
                {
                    dbContext.Users.Remove(user);
                }
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
        private List<IdentityUser> testUsers = new List<IdentityUser>()
        {
            new IdentityUser { Id = "cb8efebe-4621-4295-8105-345e43edebc0",
                UserName = "pmUser1", Email = "pmUser1@gmail.com" },
            new IdentityUser { Id = "fff5ed0c-b807-4a1e-8f3c-2e504a04f524",
                UserName = "pmUser2", Email = "pmUser2@gmail.com" },
            new IdentityUser { Id = "e7c66252-cdeb-4264-ac30-b7f81a9a62ea",
                UserName = "pmUser3", Email = "pmUser3@gmail.com" },
            new IdentityUser { Id = "8161fa25-688d-4020-999f-8354613f73c0",
                UserName = "pmUser4", Email = "pmUser4@gmail.com" },
        };

        private List<DataAccess.Models.Domain.PrivateMessage> testMessages = new
            List<DataAccess.Models.Domain.PrivateMessage>()
        {
            new DataAccess.Models.Domain.PrivateMessage(){
                SenderUserId = "cb8efebe-4621-4295-8105-345e43edebc0",
                ReceiverUserId = "fff5ed0c-b807-4a1e-8f3c-2e504a04f524",
                SentTime = DateTime.Now, Text = "text1"
            },
            new DataAccess.Models.Domain.PrivateMessage(){
                SenderUserId = "fff5ed0c-b807-4a1e-8f3c-2e504a04f524",
                ReceiverUserId = "cb8efebe-4621-4295-8105-345e43edebc0",
                SentTime = DateTime.Now, Text = "text2"
            },
            new DataAccess.Models.Domain.PrivateMessage(){
                SenderUserId = "cb8efebe-4621-4295-8105-345e43edebc0",
                ReceiverUserId = "fff5ed0c-b807-4a1e-8f3c-2e504a04f524",
                SentTime = DateTime.Now, Text = "text3"
            },

            new DataAccess.Models.Domain.PrivateMessage(){
                SenderUserId = "e7c66252-cdeb-4264-ac30-b7f81a9a62ea",
                ReceiverUserId = "cb8efebe-4621-4295-8105-345e43edebc0",
                SentTime = DateTime.Now, Text = "text4"
            },
            new DataAccess.Models.Domain.PrivateMessage(){
                SenderUserId = "cb8efebe-4621-4295-8105-345e43edebc0",
                ReceiverUserId = "e7c66252-cdeb-4264-ac30-b7f81a9a62ea",
                SentTime = DateTime.Now, Text = "text5"
            },

            new DataAccess.Models.Domain.PrivateMessage(){
                SenderUserId = "e7c66252-cdeb-4264-ac30-b7f81a9a62ea",
                ReceiverUserId = "8161fa25-688d-4020-999f-8354613f73c0",
                SentTime = DateTime.Now, Text = "text6"
            },

            new DataAccess.Models.Domain.PrivateMessage(){
                SenderUserId = "8161fa25-688d-4020-999f-8354613f73c0",
                ReceiverUserId = "cb8efebe-4621-4295-8105-345e43edebc0",
                SentTime = DateTime.Now, Text = "text7"
            },
        };
    }
}
