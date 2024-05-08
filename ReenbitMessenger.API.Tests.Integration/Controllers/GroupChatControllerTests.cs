using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ReenbitMessenger.API.Tests.Integration.TestUtils;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Net;
using System.Net.Http.Headers;

namespace ReenbitMessenger.API.Tests.Integration.Controllers
{
    public class GroupChatControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public GroupChatControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost:7051/groupchat/");

            _factory = factory;
            _httpClient = factory.CreateClient();

            AddTestData().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GetUserGroupChats_ValidRequest_ReturnsUserGroupChatsList()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userMembership = testMembers.Where(mem => mem.UserId == testUser.Id);
            var userChats = testGroupChats.Where(chat => userMembership.Any(mem => mem.GroupChatId == chat.Id));

            GetGroupChatsRequest validRequest = new GetGroupChatsRequest
            {
                Page = 0,
                ValueContains = "",
                NumberOfGroupChats = 20,
                Ascending = true
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("userGroupChats", validRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var chats = JsonConvert.DeserializeObject<List<GroupChat>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(chats);
            Assert.NotEmpty(chats);
            Assert.Equal(userMembership.Count(), chats.Count);
        }

        [Fact]
        public async Task GetUserGroupChats_InvalidToken_ReturnsBadRequestResult()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetInvalidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            GetGroupChatsRequest validRequest = new GetGroupChatsRequest
            {
                Page = 0,
                ValueContains = "",
                NumberOfGroupChats = 20,
                Ascending = true
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("userGroupChats", validRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateGroupChat_ValidRequest_ReturnsCreatedChat()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateGroupChatRequest validRequest = new CreateGroupChatRequest
            {
                Name = "newTestChat"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("", validRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var resultChat = JsonConvert.DeserializeObject<GroupChat>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultChat);
            Assert.Equal(validRequest.Name, resultChat.Name);
        }

        [Fact]
        public async Task CreateGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateGroupChatRequest validRequest = new CreateGroupChatRequest
            {
                Name = ""
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("", validRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetGroupChatById_ValidGroupChatId_ReturnsGroupChat()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var testChat = testGroupChats[0];
            var testChatMembers = testMembers.Where(mem => mem.GroupChatId == testChat.Id);

            var chatId = testChat.Id;

            // Act
            var response = await _httpClient.GetAsync($"{chatId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var resultChat = JsonConvert.DeserializeObject<GroupChat>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultChat);
            Assert.Equal(testChat.Name, resultChat.Name);

            Assert.NotNull(resultChat.GroupChatMembers);
            Assert.Equal(testChatMembers.Count(), resultChat.GroupChatMembers.Count());

            Assert.NotNull(resultChat.GroupChatMessages);
        }

        [Fact]
        public async Task GetGroupChatById_InvalidGroupChatId_ReturnsNull()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("28ee9c1d-ee94-4281-a097-73a94e4046bb");

            // Act
            var response = await _httpClient.GetAsync($"{chatId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var resultChat = JsonConvert.DeserializeObject<GroupChat>(await response.Content.ReadAsStringAsync());

            Assert.Null(resultChat);
        }

        [Fact]
        public async Task EditGroupChatInfoById_ValidRequest_ReturnsEditedGroupChat()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditGroupChatRequest validRequest = new EditGroupChatRequest
            {
                Name = "updatedChat"
            };

            var testChat = testGroupChats[1];
            var chatId = testChat.Id;

            // Act
            var response = await _httpClient.PutAsJsonAsync($"{chatId}/editInfo", validRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var resultChat = JsonConvert.DeserializeObject<GroupChat>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultChat);
            Assert.Equal(validRequest.Name, resultChat.Name);
        }

        [Fact]
        public async Task EditGroupChatInfoById_InvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditGroupChatRequest validRequest = new EditGroupChatRequest
            {
                Name = ""
            };

            var chatId = new Guid("00fd0418-345d-4630-afbd-de86fb01dc5d");

            // Act
            var response = await _httpClient.PutAsJsonAsync($"{chatId}/editInfo", validRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task JoinGroupChat_ValidRequest_ReturnsGroupChatMemberList()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var testChat = testGroupChats[3];
            var chatId = testChat.Id;

            var existingMembers = testMembers.Where(mem => mem.GroupChatId == chatId);

            // Act
            var membersResponse = await _httpClient.GetAsync($"{chatId}/join");
            var groupChatResponse = await _httpClient.GetAsync($"{chatId}");

            // Assert
            membersResponse.EnsureSuccessStatusCode();
            groupChatResponse.EnsureSuccessStatusCode();

            var resultMembers = JsonConvert
                .DeserializeObject<List<GroupChatMember>>(await membersResponse.Content.ReadAsStringAsync());

            var resultGroupChat = JsonConvert.DeserializeObject<GroupChat>(await groupChatResponse.Content.ReadAsStringAsync());

            Assert.NotNull(resultMembers);
            Assert.NotEmpty(resultMembers);
            Assert.Equal(existingMembers.Count() + resultMembers.Count(), resultGroupChat.GroupChatMembers.Count());
        }

        [Fact]
        public async Task JoinGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe6-9d52-4a90-a227-1011a2aea296");

            // Act
            var response = await _httpClient.GetAsync($"{chatId}/join");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task LeaveGroupChat_ValidRequest_ReturnsSuccessStatusCode()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var testChat = testGroupChats[0];
            var chatId = testChat.Id;

            // Act
            var response = await _httpClient.DeleteAsync($"{chatId}/leave");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task LeaveGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe6-9d52-4a90-a227-1011a2aea296");

            // Act
            var response = await _httpClient.DeleteAsync($"{chatId}/leave");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteGroupChatById_ValidRequest_ReturnsSuccessStatusCode()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var testChat = testGroupChats[0];
            var chatId = testChat.Id;

            // Act
            var response = await _httpClient.DeleteAsync($"{chatId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteGroupChatById_InvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe5-9d52-4a90-a227-1011a2aea296");

            // Act
            var response = await _httpClient.DeleteAsync($"{chatId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private async Task AddTestData()
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

            foreach (var gc in testGroupChats)
            {
                if (!dbContext.GroupChat.Contains(gc))
                {
                    dbContext.GroupChat.Add(gc);
                }
            }

            foreach (var member in testMembers)
            {
                if (!dbContext.GroupChatMember.Contains(member))
                {
                    dbContext.GroupChatMember.Add(member);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task ClearTestData()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();

            foreach (var member in dbContext.GroupChatMember)
            {
                dbContext.GroupChatMember.Remove(member);
            }

            foreach (var gc in dbContext.GroupChat)
            {
                dbContext.GroupChat.Remove(gc);
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

            ClearTestData().GetAwaiter().GetResult();
        }

        // Test data
        private List<IdentityUser> testUsers = new List<IdentityUser>()
        {
            new IdentityUser { Id = "33db7d2a-1cd0-4c30-a7a4-555d75b69a8d",
                UserName = "testUser1", Email = "testUser1@gmail.com" },
            new IdentityUser { Id = "c71f3be1-eb29-47a5-8319-0b703116838b",
                UserName = "testUser2", Email = "testUser2@gmail.com" },
            new IdentityUser { Id = "c8cfd3ac-4415-421d-953b-96a675f708fa",
                UserName = "testUser3", Email = "testUser3@gmail.com" },
            new IdentityUser { Id = "7d84eec2-bd68-4107-afdb-e7aac41544ce",
                UserName = "testUser4", Email = "testUser4@gmail.com" },
        };

        private List<DataAccess.Models.Domain.GroupChat> testGroupChats = new List<DataAccess.Models.Domain.GroupChat>()
        {
            new DataAccess.Models.Domain.GroupChat { 
                Id = new Guid("28ee8c1d-ee94-4281-a097-73a94e4046bb"), Name = "testChat1" },
            new DataAccess.Models.Domain.GroupChat {
                Id = new Guid("9f3ff815-767f-4836-8382-b4d3b0c25994"), Name = "testChat2" },
            new DataAccess.Models.Domain.GroupChat {
                Id = new Guid("3fd12db3-11e0-49b3-8d61-5f60b1f37e21"), Name = "testChat3" },
            new DataAccess.Models.Domain.GroupChat {
                Id = new Guid("fb9728f4-f590-41d7-9f75-36355165a9fe"), Name = "testChat4" },
            new DataAccess.Models.Domain.GroupChat {
                Id = new Guid("e644041d-8b8f-4813-9a0a-093ead49727e"), Name = "testChat5" },
        };

        private List<DataAccess.Models.Domain.GroupChatMember> testMembers =
            new List<DataAccess.Models.Domain.GroupChatMember>
        {
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("28ee8c1d-ee94-4281-a097-73a94e4046bb"), GroupChatRoleId = 1,
                UserId = "33db7d2a-1cd0-4c30-a7a4-555d75b69a8d" },
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("28ee8c1d-ee94-4281-a097-73a94e4046bb"), GroupChatRoleId = 1,
                UserId = "c71f3be1-eb29-47a5-8319-0b703116838b"},
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("28ee8c1d-ee94-4281-a097-73a94e4046bb"), GroupChatRoleId = 1,
                UserId = "c8cfd3ac-4415-421d-953b-96a675f708fa"},

            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("9f3ff815-767f-4836-8382-b4d3b0c25994"), GroupChatRoleId = 1,
                UserId = "33db7d2a-1cd0-4c30-a7a4-555d75b69a8d" },
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("9f3ff815-767f-4836-8382-b4d3b0c25994"), GroupChatRoleId = 1,
                UserId = "c71f3be1-eb29-47a5-8319-0b703116838b"},

            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("3fd12db3-11e0-49b3-8d61-5f60b1f37e21"), GroupChatRoleId = 1,
                UserId = "c71f3be1-eb29-47a5-8319-0b703116838b"},
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("3fd12db3-11e0-49b3-8d61-5f60b1f37e21"), GroupChatRoleId = 1,
                UserId = "c8cfd3ac-4415-421d-953b-96a675f708fa"},

            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("fb9728f4-f590-41d7-9f75-36355165a9fe"), GroupChatRoleId = 1,
                UserId = "c8cfd3ac-4415-421d-953b-96a675f708fa"}
        };
    }
}
