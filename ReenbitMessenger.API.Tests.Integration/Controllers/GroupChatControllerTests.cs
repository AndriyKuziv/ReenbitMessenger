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
    public class GroupChatControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<GroupChatTestFixture>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        private GroupChatTestFixture _fixture;

        protected MessengerDataContext DbContext;
        protected TransactionScope TransactionScope;

        public GroupChatControllerTests(CustomWebApplicationFactory<Program> factory, GroupChatTestFixture fixture)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost:7051/groupchat/");

            _factory = factory;
            _httpClient = factory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            _fixture = fixture;
        }

        [Fact]
        public async Task GetUserGroupChats_ValidRequest_ReturnsOkResult_UserGroupChatList()
        {
            var token = TestsHelper.GetValidToken();

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
            var token = TestsHelper.GetInvalidToken();

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

        [Fact]
        public async Task CreateGroupChat_ValidRequest_ReturnsCreatedChat()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateGroupChatRequest validRequest = new CreateGroupChatRequest
            {
                Name = "newTestChat"
            };

            var response = await _httpClient.PostAsJsonAsync("", validRequest);

            response.EnsureSuccessStatusCode();

            var resultChat = JsonConvert.DeserializeObject<GroupChat>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultChat);

            _fixture.ChatId = resultChat.Id;
        }

        [Fact]
        public async Task CreateGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            var token = TestsHelper.GetValidToken();

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
            var token = TestsHelper.GetValidToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = _fixture.ChatId;

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
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditGroupChatRequest validRequest = new EditGroupChatRequest
            {
                Name = "updatedChat"
            };

            var chatId = _fixture.ChatId;

            var response = await _httpClient.PutAsJsonAsync($"{chatId}/editInfo", validRequest);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task EditGroupChatInfoById_InvalidRequest_ReturnsBadRequestResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            EditGroupChatRequest validRequest = new EditGroupChatRequest
            {
                Name = ""
            };

            var chatId = _fixture.ChatId;

            var response = await _httpClient.PutAsJsonAsync($"{chatId}/editInfo", validRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task JoinGroupChat_ValidRequest_ReturnsGroupChatMemberList()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = _fixture.ChatId;

            var response = await _httpClient.GetAsync($"{chatId}/join");

            response.EnsureSuccessStatusCode();

            var resultMembers = JsonConvert
                .DeserializeObject<List<GroupChatMember>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(resultMembers);
        }

        [Fact]
        public async Task JoinGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe5-9d52-4a90-a227-1011a2aea296");

            var response = await _httpClient.GetAsync($"{chatId}/join");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task LeaveGroupChat_ValidRequest_ReturnsOkResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = _fixture.ChatId;

            var response = await _httpClient.DeleteAsync($"{chatId}/leave");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task LeaveGroupChat_InvalidRequest_ReturnsBadRequestResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe5-9d52-4a90-a227-1011a2aea296");

            var response = await _httpClient.DeleteAsync($"{chatId}/leave");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteGroupChatById_ValidRequest_ReturnsOkResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = _fixture.ChatId;

            var response = await _httpClient.DeleteAsync($"{chatId}");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteGroupChatById_InvalidRequest_ReturnsBadRequestResult()
        {
            var token = TestsHelper.GetValidToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var chatId = new Guid("fea4bbe5-9d52-4a90-a227-1011a2aea296");

            var response = await _httpClient.DeleteAsync($"{chatId}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
