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
    public class PrivateMessageRepositoryTests : IDisposable
    {
        IQueryable<PrivateMessage> testData = new List<PrivateMessage>()
            {
                new PrivateMessage() { Id = 1, ReceiverUserId = "user2", SenderUserId = "user1", SentTime = DateTime.Now, Text = "Hello!" },
                new PrivateMessage() { Id = 2, MessageToReplyId = 1, ReceiverUserId = "user1", SenderUserId = "user2", SentTime = DateTime.Now, Text = "Hi" },
                new PrivateMessage() { Id = 3, MessageToReplyId = 2, ReceiverUserId = "user2", SenderUserId = "user1", SentTime = DateTime.Now, Text = "How are you today" },
                new PrivateMessage() { Id = 4, MessageToReplyId = 2, ReceiverUserId = "user1", SenderUserId = "user2", SentTime = DateTime.Now, Text = "I'm fine, thanks" },
                new PrivateMessage() { Id = 5, ReceiverUserId = "user2", SenderUserId = "user1", SentTime = DateTime.Now, Text = "Glad to hear that" },
            }.AsQueryable();

        DbContextOptions<MessengerDataContext> options = new DbContextOptionsBuilder<MessengerDataContext>()
            .UseInMemoryDatabase(databaseName: "PrivateMessage")
            .Options;

        MessengerDataContext context;

        public PrivateMessageRepositoryTests()
        {
            context = new MessengerDataContext(options);

            context.Database.EnsureDeleted();
            context.AddRange(testData);
            context.SaveChanges();
        }

        [Fact]
        public async Task UpdateAsync_ReturnsPrivateMessage()
        {
            // Arrange
            var repository = new PrivateMessageRepository(context);
            var originalMessage = testData.First();
            var updatedMessage = new PrivateMessage() { Text = "updatedText" };

            // Act
            var resultMessage = await repository.UpdateAsync(originalMessage.Id, updatedMessage);

            // Assert
            Assert.NotNull(resultMessage);
            Assert.Equal(updatedMessage.Text, resultMessage.Text);
        }

        [Fact]
        public async Task GetPrivateChatAsync_ReturnsPrivateMessagesBetweenTwoUsers()
        {
            // Arrange
            var repository = new PrivateMessageRepository(context);
            string firstUserId = "user1";
            string secondUserId = "user2";

            // Act
            var resultPrivateMessagesList = await repository.GetPrivateChatAsync(firstUserId, secondUserId);

            // Assert
            Assert.NotNull(resultPrivateMessagesList);
            Assert.NotEmpty(resultPrivateMessagesList);
            Assert.True(resultPrivateMessagesList.All(pm => 
                    (pm.SenderUserId == firstUserId && pm.ReceiverUserId == secondUserId) || 
                    (pm.SenderUserId == secondUserId && pm.ReceiverUserId == firstUserId)
            ));
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
