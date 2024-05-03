using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using ReenbitMessenger.API.Tests.Integration.TestUtils;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System.Diagnostics.CodeAnalysis;

namespace ReenbitMessenger.API.Tests.Integration.Hubs
{
    public class ChatHubTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;

        private HubConnection? _hubConnection;

        public ChatHubTests(CustomWebApplicationFactory<Program> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost:7051/groupchat/");

            _factory = factory;
            _httpClient = factory.CreateClient();

            AddTestData().GetAwaiter().GetResult();
        }

        private static async Task<HubConnection> StartConnectionAsync(HttpMessageHandler handler, string accessToken)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:7051/chathub", options =>
                {
                    options.HttpMessageHandlerFactory = _ => handler;
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();

            await hubConnection.StartAsync();

            return hubConnection;
        }

        [Fact]
        public async Task GetGroupChatInfo_ValidGroupChatId_SendsGroupChatInfo()
        {
            // Arrange
            var token = TestsHelper.GetValidToken(testUsers[0]);

            _hubConnection = await StartConnectionAsync(_factory.Server.CreateHandler(), token);

            var expectedChat = testGroupChats[1];

            GroupChat resultChat = null;

            _hubConnection.On<GroupChat>("ReceiveGroupChatInfo", (groupChat) =>
            {
                resultChat = groupChat;
            });

            // Act
            //await _hubConnection.SendAsync("GetGroupChatInfo", Convert.ToString(expectedChat.Id));
            await _hubConnection.InvokeAsync("GetGroupChatInfo", Convert.ToString(expectedChat.Id));

            await Task.Delay(500);

            // Assert
            Assert.NotNull(resultChat);
            Assert.Equal(expectedChat.Id, resultChat.Id);
            Assert.Equal(expectedChat.Name, resultChat.Name);
        }

        [Fact]
        public async Task GetGroupChatMessages_ValidRequest_SendsGroupChatMessages()
        {
            // Arrange
            var token = TestsHelper.GetValidToken(testUsers[0]);

            _hubConnection = await StartConnectionAsync(_factory.Server.CreateHandler(), token);

            var expectedChat = testGroupChats[1];
            var expectedMessages = testMessages.Where(msg => msg.GroupChatId == expectedChat.Id);

            IEnumerable<GroupChatMessage> resultMessages = null;

            _hubConnection.On<IEnumerable<GroupChatMessage>>("ReceiveGroupChatMessages", (groupChatMessages) =>
            {
                resultMessages = groupChatMessages;
            });

            // Act
            await _hubConnection.InvokeAsync("GetGroupChatMessages", Convert.ToString(expectedChat.Id), 0, 20, "", true);

            await Task.Delay(500);

            Assert.NotNull(resultMessages);
            Assert.Equal(expectedMessages.Count(), resultMessages.Count());
        }

        [Fact]
        public async Task CreateGroupChat_ValidRequest_SendsCreatedGroupChat()
        {
            // Arrange
            var token = TestsHelper.GetValidToken(testUsers[0]);

            _hubConnection = await StartConnectionAsync(_factory.Server.CreateHandler(), token);

            CreateGroupChatRequest validRequest = new CreateGroupChatRequest
            {
                Name = "newChat"
            };

            GroupChat resultChat = null;

            _hubConnection.On<GroupChat>("ReceiveGroupChat", (groupChat) =>
            {
                resultChat = groupChat;
            });

            // Act
            await _hubConnection.InvokeAsync("CreateGroupChat", validRequest);

            await Task.Delay(500);

            Assert.NotNull(resultChat);
            Assert.Equal(validRequest.Name, resultChat.Name);
        }

        [Fact]
        public async Task SendGroupChatMessage_ValidRequest_SendsNewGroupChatMessage()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _hubConnection = await StartConnectionAsync(_factory.Server.CreateHandler(), token);

            var groupChat = testGroupChats[0];
            SendMessageToGroupChatRequest validRequest = new SendMessageToGroupChatRequest
            {
                Text = "newMessage"
            };

            GroupChatMessage resultMessage = null;

            _hubConnection.On<GroupChatMessage>("ReceiveMessage", (message) =>
            {
                resultMessage = message;
            });

            await _hubConnection.InvokeAsync("ConnectToGroupChat", Convert.ToString(groupChat.Id));

            // Act
            await _hubConnection.InvokeAsync("SendGroupChatMessage", Convert.ToString(groupChat.Id), validRequest);

            await Task.Delay(500);

            Assert.NotNull(resultMessage);
            Assert.Equal(validRequest.Text, resultMessage.Text);
            Assert.Equal(validRequest.MessageToReplyId, resultMessage.MessageToReplyId);
            Assert.Equal(testUser.Id, resultMessage.SenderUserId);
            Assert.Equal(groupChat.Id, resultMessage.GroupChatId);
        }

        [Fact]
        public async Task DeleteGroupChatMessage_ValidRequest_SendsDeletedGroupChatMessage()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);
            var groupChat = testGroupChats[0];

            _hubConnection = await StartConnectionAsync(_factory.Server.CreateHandler(), token);

            IEnumerable<GroupChatMessage> groupChatMessages = null;

            _hubConnection.On<IEnumerable<GroupChatMessage>>("ReceiveGroupChatMessages", (messages) =>
            {
                groupChatMessages = messages;
            });

            await _hubConnection.InvokeAsync("GetGroupChatMessages", Convert.ToString(groupChat.Id), 0, 20, "", true);

            await Task.Delay(500);

            var expectedMessage = groupChatMessages.First();

            DeleteMessageFromGroupChatRequest validRequest = new DeleteMessageFromGroupChatRequest
            {
                MessageId = expectedMessage.Id,
            };

            GroupChatMessage resultMessage = null;

            _hubConnection.On<GroupChatMessage>("DeleteMessage", (message) =>
            {
                resultMessage = message;
            });

            await _hubConnection.InvokeAsync("ConnectToGroupChat", Convert.ToString(groupChat.Id));

            // Act
            await _hubConnection.InvokeAsync("DeleteGroupChatMessage", Convert.ToString(groupChat.Id), validRequest);

            await Task.Delay(500);

            // Assert
            Assert.NotNull(resultMessage);
            Assert.Equal(expectedMessage.Id, resultMessage.Id);
        }

        [Fact]
        public async Task AddUsersToGroupChat_ValidRequest_SendsNewGroupChatMembers()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _hubConnection = await StartConnectionAsync(_factory.Server.CreateHandler(), token);

            var groupChat = testGroupChats[1];
            var existingMembers = testMembers.Where(mem => mem.GroupChatId == groupChat.Id);
            AddUsersToGroupChatRequest validRequest = new AddUsersToGroupChatRequest
            {
                UsersIds = new List<string> { "0db5a5a8-8c25-4e90-8a64-2709d8304565" }
            };

            IEnumerable<GroupChatMember> addedMembers = null;

            _hubConnection.On<IEnumerable<GroupChatMember>>("ReceiveMembers", (members) =>
            {
                addedMembers = members;
            });

            await _hubConnection.InvokeAsync("ConnectToGroupChat", Convert.ToString(groupChat.Id));

            // Act
            await _hubConnection.InvokeAsync("AddUsersToGroupChat", Convert.ToString(groupChat.Id), validRequest);

            await Task.Delay(500);

            // Assert
            Assert.NotNull(addedMembers);

            GroupChat resultGroupChat = null;

            _hubConnection.On<GroupChat>("ReceiveGroupChatInfo", (groupChat) =>
            {
                resultGroupChat = groupChat;
            });

            await _hubConnection.InvokeAsync("GetGroupChatInfo", Convert.ToString(groupChat.Id));

            await Task.Delay(500);

            Assert.Equal(resultGroupChat.GroupChatMembers.Count(), existingMembers.Count() + addedMembers.Count());
        }

        [Fact]
        public async Task RemoveUsersFromGroupChat_ValidRequest_SendsRemovedGroupChatMembersIds()
        {
            // Arrange
            var testUser = testUsers[0];
            var token = TestsHelper.GetValidToken(testUser);

            _hubConnection = await StartConnectionAsync(_factory.Server.CreateHandler(), token);

            var groupChat = testGroupChats[1];
            var existingMembers = testMembers.Where(mem => mem.GroupChatId == groupChat.Id);
            RemoveUsersFromGroupChatRequest validRequest = new RemoveUsersFromGroupChatRequest
            {
                UsersIds = new List<string> { testUser.Id }
            };

            IEnumerable<string> removedMembersIds = null;
            _hubConnection.On<IEnumerable<string>>("RemoveMembers", (members) =>
            {
                removedMembersIds = members;
            });

            await _hubConnection.InvokeAsync("ConnectToGroupChat", Convert.ToString(groupChat.Id));

            // Act
            await _hubConnection.InvokeAsync("RemoveUsersFromGroupChat", Convert.ToString(groupChat.Id), validRequest);

            await Task.Delay(500);

            // Assert
            Assert.NotNull(removedMembersIds);

            GroupChat resultGroupChat = null;

            _hubConnection.On<GroupChat>("ReceiveGroupChatInfo", (groupChat) =>
            {
                resultGroupChat = groupChat;
            });

            await _hubConnection.InvokeAsync("GetGroupChatInfo", Convert.ToString(groupChat.Id));

            await Task.Delay(500);

            Assert.Equal(resultGroupChat.GroupChatMembers.Count(), existingMembers.Count() - removedMembersIds.Count());
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

            foreach (var message in testMessages)
            {
                if (!dbContext.GroupChatMessage.Contains(message))
                {
                    dbContext.GroupChatMessage.Add(message);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task ClearTestData()
        {
            using var scope = _factory.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<MessengerDataContext>();

            foreach (var message in dbContext.GroupChatMessage)
            {
                dbContext.GroupChatMessage.Remove(message);
            }

            foreach (var member in dbContext.GroupChatMember)
            {
                dbContext.GroupChatMember.Remove(member);
            }

            foreach (var gc in dbContext.GroupChat)
            {
                dbContext.GroupChat.Remove(gc);
            }

            foreach (var user in dbContext.Users)
            {
                dbContext.Users.Remove(user);
            }

            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_hubConnection != null)
            {
                _hubConnection.StopAsync().GetAwaiter().GetResult();
                _hubConnection.DisposeAsync().GetAwaiter().GetResult();
            }
            if ( _httpClient != null)
            {
                _httpClient.Dispose();
            }

            ClearTestData().GetAwaiter().GetResult();
        }

        private List<IdentityUser> testUsers = new List<IdentityUser>()
        {
            new IdentityUser { Id = "1aaa6349-79a4-4637-b990-3adf0005c288",
                UserName = "hubTestUser1", Email = "hubTestUser1@gmail.com" },
            new IdentityUser { Id = "0db5a5a8-8c25-4e90-8a64-2709d8304565",
                UserName = "hubTestUser2", Email = "hubTestUser2@gmail.com" },
            new IdentityUser { Id = "5d70ef57-4e55-40bc-9780-3fbb6b33ea00",
                UserName = "hubTestUser3", Email = "hubTestUser3@gmail.com" }
        };

        private List<DataAccess.Models.Domain.GroupChat> testGroupChats = new List<DataAccess.Models.Domain.GroupChat>()
        {
            new DataAccess.Models.Domain.GroupChat {
                Id = new Guid("b59339ac-5cde-46db-b615-2d29d7fa7ab6"), Name = "testChat1" },
            new DataAccess.Models.Domain.GroupChat {
                Id = new Guid("966f1db1-276f-4f67-8407-74f075f2c8e2"), Name = "testChat2" }
        };

        private List<DataAccess.Models.Domain.GroupChatMember> testMembers =
            new List<DataAccess.Models.Domain.GroupChatMember>
        {
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("b59339ac-5cde-46db-b615-2d29d7fa7ab6"), GroupChatRoleId = 1,
                UserId = "1aaa6349-79a4-4637-b990-3adf0005c288" },
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("b59339ac-5cde-46db-b615-2d29d7fa7ab6"), GroupChatRoleId = 1,
                UserId = "0db5a5a8-8c25-4e90-8a64-2709d8304565"},
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("b59339ac-5cde-46db-b615-2d29d7fa7ab6"), GroupChatRoleId = 1,
                UserId = "5d70ef57-4e55-40bc-9780-3fbb6b33ea00"},

            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("966f1db1-276f-4f67-8407-74f075f2c8e2"), GroupChatRoleId = 1,
                UserId = "1aaa6349-79a4-4637-b990-3adf0005c288" },
            new DataAccess.Models.Domain.GroupChatMember {
                GroupChatId = new Guid("966f1db1-276f-4f67-8407-74f075f2c8e2"), GroupChatRoleId = 1,
                UserId = "5d70ef57-4e55-40bc-9780-3fbb6b33ea00"}
        };

        private List<DataAccess.Models.Domain.GroupChatMessage> testMessages =
            new List<DataAccess.Models.Domain.GroupChatMessage>
            {
                new DataAccess.Models.Domain.GroupChatMessage {
                    GroupChatId = new Guid("b59339ac-5cde-46db-b615-2d29d7fa7ab6"), SenderUserId = "1aaa6349-79a4-4637-b990-3adf0005c288", SentTime = DateTime.Now, Text = "text1"
                },
                new DataAccess.Models.Domain.GroupChatMessage {
                    GroupChatId = new Guid("b59339ac-5cde-46db-b615-2d29d7fa7ab6"), SenderUserId = "0db5a5a8-8c25-4e90-8a64-2709d8304565", SentTime = DateTime.Now, Text = "text2"
                },
                new DataAccess.Models.Domain.GroupChatMessage {
                    GroupChatId = new Guid("b59339ac-5cde-46db-b615-2d29d7fa7ab6"), SenderUserId = "0db5a5a8-8c25-4e90-8a64-2709d8304565", SentTime = DateTime.Now, Text = "text3"
                },
                new DataAccess.Models.Domain.GroupChatMessage {
                    GroupChatId = new Guid("b59339ac-5cde-46db-b615-2d29d7fa7ab6"), SenderUserId = "1aaa6349-79a4-4637-b990-3adf0005c288", SentTime = DateTime.Now, Text = "text4"
                },

                new DataAccess.Models.Domain.GroupChatMessage {
                    GroupChatId = new Guid("966f1db1-276f-4f67-8407-74f075f2c8e2"), SenderUserId = "1aaa6349-79a4-4637-b990-3adf0005c288", SentTime = DateTime.Now, Text = "text5"
                },
                new DataAccess.Models.Domain.GroupChatMessage {
                    GroupChatId = new Guid("966f1db1-276f-4f67-8407-74f075f2c8e2"), SenderUserId = "5d70ef57-4e55-40bc-9780-3fbb6b33ea00", SentTime = DateTime.Now, Text = "text6"
                },
            };
    }
}
