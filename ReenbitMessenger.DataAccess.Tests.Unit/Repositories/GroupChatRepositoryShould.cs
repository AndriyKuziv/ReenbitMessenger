using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Tests.Unit.Repositories
{
    public class GroupChatRepositoryShould : IDisposable
    {
        IQueryable<GroupChat> groupChatTestData = new List<GroupChat>()
            {
                new GroupChat() { Id = new Guid("c5f89626-28ed-4acb-990a-8eddcfa67219"), Name = "chat1" },
                new GroupChat() { Id = new Guid("83967693-6c60-4f5e-9b3a-d8b6b715059d"), Name = "chat2" },
                new GroupChat() { Id = new Guid("68341f25-c6de-4d04-90be-0ca0bd09328a"), Name = "chat3" },
            }.AsQueryable();

        IQueryable<GroupChatMember> memberTestData = new List<GroupChatMember>()
        {
            new GroupChatMember() { Id = 1, GroupChatId = new Guid("c5f89626-28ed-4acb-990a-8eddcfa67219"), GroupChatRoleId = 1, UserId = "user1" },
            new GroupChatMember() { Id = 2, GroupChatId = new Guid("c5f89626-28ed-4acb-990a-8eddcfa67219"), GroupChatRoleId = 1, UserId = "user2" },
            new GroupChatMember() { Id = 3, GroupChatId = new Guid("c5f89626-28ed-4acb-990a-8eddcfa67219"), GroupChatRoleId = 1, UserId = "user3" },
            new GroupChatMember() { Id = 4, GroupChatId = new Guid("83967693-6c60-4f5e-9b3a-d8b6b715059d"), GroupChatRoleId = 1, UserId = "user1" },
            new GroupChatMember() { Id = 5, GroupChatId = new Guid("68341f25-c6de-4d04-90be-0ca0bd09328a"), GroupChatRoleId = 1, UserId = "user1" },
            new GroupChatMember() { Id = 6, GroupChatId = new Guid("68341f25-c6de-4d04-90be-0ca0bd09328a"), GroupChatRoleId = 1, UserId = "user2" },
        }.AsQueryable();

        IQueryable<GroupChatMessage> messageTestData = new List<GroupChatMessage>()
        {
            new GroupChatMessage() { Id = 1, GroupChatId = new Guid("c5f89626-28ed-4acb-990a-8eddcfa67219"), SenderUserId = "user1", SentTime = DateTime.Now, Text = "test1" },
            new GroupChatMessage() { Id = 2, GroupChatId = new Guid("c5f89626-28ed-4acb-990a-8eddcfa67219"), SenderUserId = "user2", SentTime = DateTime.Now, Text = "test2" },
            new GroupChatMessage() { Id = 3, GroupChatId = new Guid("c5f89626-28ed-4acb-990a-8eddcfa67219"), SenderUserId = "user3", SentTime = DateTime.Now, Text = "test3" },
            new GroupChatMessage() { Id = 4, GroupChatId = new Guid("c5f89626-28ed-4acb-990a-8eddcfa67219"), SenderUserId = "user1", SentTime = DateTime.Now, Text = "test4" },
            new GroupChatMessage() { Id = 5, GroupChatId = new Guid("68341f25-c6de-4d04-90be-0ca0bd09328a"), SenderUserId = "user2", SentTime = DateTime.Now, Text = "test5" },
            new GroupChatMessage() { Id = 6, GroupChatId = new Guid("83967693-6c60-4f5e-9b3a-d8b6b715059d"), SenderUserId = "user1", SentTime = DateTime.Now, Text = "test6" },
        }.AsQueryable();

        DbContextOptions<MessengerDataContext> options = new DbContextOptionsBuilder<MessengerDataContext>()
            .UseInMemoryDatabase(databaseName: "GroupChat")
            .Options;

        MessengerDataContext context;
        GroupChatRepository repository;

        public GroupChatRepositoryShould()
        {
            context = new MessengerDataContext(options);

            context.Database.EnsureDeleted();
            context.AddRange(groupChatTestData);
            context.AddRange(memberTestData);
            context.AddRange(messageTestData);
            context.SaveChanges();

            repository = new GroupChatRepository(context);
        }

        // Group chats tests
        [Fact]
        public async Task GetFullAsync_ReturnsFullGroupChat()
        {
            // Arrange
            var chatId = context.GroupChat.First().Id;

            // Act
            var resultGroupChat = await repository.GetFullAsync(chatId);

            // Assert
            Assert.NotNull(resultGroupChat);
            Assert.Equal(chatId, resultGroupChat.Id);
        }

        [Fact]
        public async Task GetUserChats_ReturnsUserChats()
        {
            // Arrange
            var userId = "user1";

            // Act
            var resultUserChats = await repository.GetUserChatsAsync(userId);

            // Assert
            Assert.NotNull(resultUserChats);
            Assert.NotEmpty(resultUserChats);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedChat()
        {
            // Arrange

            // Act

            // Assert
        }

        // Members tests
        [Fact]
        public async Task GetMemberAsync_ReturnsGroupChatMember()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task FilterMembersAsync_ReturnsFilteredMembers()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task GetGroupChatMembersAsync_ReturnsGroupChatMembers()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task IsInGroupChat_ReturnsTrueIfInGroupChat()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task AddUserToGroupChatAsync_AddsNewUserToGroupChat()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task RemoveUserFromGroupChatAsync_RemovesExistingGroupChatMember()
        {
            // Arrange

            // Act

            // Assert
        }

        // Messages tests
        [Fact]
        public async Task GetMessageAsync_ReturnsGroupChatMessage()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task GetMessagesAsync_ReturnsGroupChatMessages()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task GetMessageHistoryAsync_ReturnsGroupChatMessagesHistory()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task CreateGroupChatMessageAsync_ReturnsCreatedGroupChatMessage()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task DeleteGroupChatMessageAsync_ReturnsDeletedGroupChatMessage()
        {
            // Arrange

            // Act

            // Assert
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
