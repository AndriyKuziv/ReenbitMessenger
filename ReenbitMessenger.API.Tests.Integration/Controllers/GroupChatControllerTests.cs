using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ReenbitMessenger.API.Tests.Integration.TestUtils;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Transactions;

namespace ReenbitMessenger.API.Tests.Integration.Controllers
{
    public class GroupChatControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        protected MessengerDataContext DbContext;
        protected TransactionScope TransactionScope;

        public GroupChatControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost:7051/groupchat/");

            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetUserGroupChats_ValidRequest_ReturnsOkResult_UserGroupChatList()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            GetGroupChatsRequest validRequest = new GetGroupChatsRequest
            {
                Page = 0,
                ValueContains = "",
                NumberOfGroupChats = 20,
                Ascending = true,
                OrderBy = "UserName"
            };

            var response = await _httpClient.PostAsJsonAsync("userGroupChats", validRequest);

            response.EnsureSuccessStatusCode();

            var chats = JsonConvert.DeserializeObject<List<GroupChat>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(chats);
            Assert.NotEmpty(chats);
        }

        [Fact]
        public async Task GetUserGroupChats_InvalidToken_ReturnsBadRequestResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            GetGroupChatsRequest validRequest = new GetGroupChatsRequest
            {
                Page = 0,
                ValueContains = "",
                NumberOfGroupChats = 20,
                Ascending = true,
                OrderBy = "UserName"
            };

            var response = await _httpClient.PostAsJsonAsync("userGroupChats", validRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        //[Fact]
        public async Task CreateGroupChat_ValidRequest_ReturnsCreatedChat()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateGroupChatRequest validRequest = new CreateGroupChatRequest
            {
                Name = "newTestChat"
            };

            var response = await _httpClient.PostAsJsonAsync("", validRequest);

            response.EnsureSuccessStatusCode();

            var resultChat = JsonConvert.DeserializeObject<GroupChat>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultChat);
        }

        //[Fact]
        public async Task CreateGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateGroupChatRequest validRequest = new CreateGroupChatRequest
            {
                Name = ""
            };

            var response = await _httpClient.PostAsJsonAsync("", validRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetGroupChatById_ValidRequest_ReturnsGroupChat()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("e6637bb6-67f9-40fe-b001-6488918488ca");

            var response = await _httpClient.GetAsync($"{chatId}");

            response.EnsureSuccessStatusCode();

            var chat = JsonConvert.DeserializeObject<GroupChat>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(chat);
            Assert.NotNull(chat.GroupChatMembers);
            Assert.NotEmpty(chat.GroupChatMembers);
            Assert.NotNull(chat.GroupChatMessages);
        }

        [Fact]
        public async Task EditGroupChatInfoById_ValidRequest_ReturnsOkResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditGroupChatRequest validRequest = new EditGroupChatRequest
            {
                Name = "updatedChat"
            };

            var chatId = new Guid("bfda2ff5-f6bb-46bf-8140-5132f38e3e22");

            var response = await _httpClient.PutAsJsonAsync($"{chatId}/editInfo", validRequest);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task EditGroupChatInfoById_InvalidRequest_ReturnsBadRequestResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditGroupChatRequest validRequest = new EditGroupChatRequest
            {
                Name = ""
            };

            var chatId = new Guid("00fd0418-345d-4630-afbd-de86fb01dc5d");

            var response = await _httpClient.PutAsJsonAsync($"{chatId}/editInfo", validRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task JoinGroupChat_ValidRequest_ReturnsGroupChatMemberList()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("e6637bb6-67f9-40fe-b001-6488918488ca");

            var response = await _httpClient.GetAsync($"{chatId}/join");

            response.EnsureSuccessStatusCode();

            var resultMembers = JsonConvert
                .DeserializeObject<List<GroupChatMember>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultMembers);
        }

        [Fact]
        public async Task JoinGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe5-9d52-4a90-a227-1011a2aea296");

            var response = await _httpClient.GetAsync($"{chatId}/join");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task LeaveGroupChat_ValidRequest_ReturnsOkResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("e6637bb6-67f9-40fe-b001-6488918488ca");

            var response = await _httpClient.DeleteAsync($"{chatId}/leave");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task LeaveGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe5-9d52-4a90-a227-1011a2aea296");

            var response = await _httpClient.DeleteAsync($"{chatId}/leave");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteGroupChatById_ValidRequest_ReturnsOkResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("");

            var response = await _httpClient.DeleteAsync($"{chatId}");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteGroupChatById_InvalidRequest_ReturnsBadRequestResult()
        {
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe5-9d52-4a90-a227-1011a2aea296");

            var response = await _httpClient.DeleteAsync($"{chatId}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private void AddTestGroupChats()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();
        }

        private List<IdentityUser> testUsers = new List<IdentityUser>()
        {
            new IdentityUser { Id = "00a11324-0a61-4893-b79f-54ba531ec2b8", UserName = "testUser1", Email = "test1@gmail.com" },
            new IdentityUser { Id = "67b53758-87aa-4f36-81fc-667c3ff16ca0", UserName = "testUser2", Email = "test2@gmail.com" },
            new IdentityUser { Id = "a0146c3a-3aa3-4bea-9917-58a7bcd3e35a", UserName = "testUser2", Email = "test1@gmail.com" },
            new IdentityUser { Id = "28d7154a-4eba-4a8f-8086-755994e9062b", UserName = "testUser2", Email = "test2@gmail.com" },
        };

        private List<DataAccess.Models.Domain.GroupChat> groupChats = new List<DataAccess.Models.Domain.GroupChat>()
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

        private List<DataAccess.Models.Domain.GroupChatMember> groupChatMembers = new List<DataAccess.Models.Domain.GroupChatMember>
        {
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("28ee8c1d-ee94-4281-a097-73a94e4046bb"), GroupChatRoleId = 1,
                UserId = "00a11324-0a61-4893-b79f-54ba531ec2b8" },
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("28ee8c1d-ee94-4281-a097-73a94e4046bb"), GroupChatRoleId = 1,
                UserId = "67b53758-87aa-4f36-81fc-667c3ff16ca0"},
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("28ee8c1d-ee94-4281-a097-73a94e4046bb"), GroupChatRoleId = 1,
                UserId = "a0146c3a-3aa3-4bea-9917-58a7bcd3e35a"},

            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("9f3ff815-767f-4836-8382-b4d3b0c25994"), GroupChatRoleId = 1,
                UserId = "00a11324-0a61-4893-b79f-54ba531ec2b8" },
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("9f3ff815-767f-4836-8382-b4d3b0c25994"), GroupChatRoleId = 1,
                UserId = "67b53758-87aa-4f36-81fc-667c3ff16ca0"},

            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("3fd12db3-11e0-49b3-8d61-5f60b1f37e21"), GroupChatRoleId = 1,
                UserId = "67b53758-87aa-4f36-81fc-667c3ff16ca0"},
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("3fd12db3-11e0-49b3-8d61-5f60b1f37e21"), GroupChatRoleId = 1,
                UserId = "a0146c3a-3aa3-4bea-9917-58a7bcd3e35a"},

            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("fb9728f4-f590-41d7-9f75-36355165a9fe"), GroupChatRoleId = 1,
                UserId = "a0146c3a-3aa3-4bea-9917-58a7bcd3e35a"}
        };
    }
}
