using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using Xunit.Sdk;
using Xunit;

namespace ReenbitMessenger.DataAccess.Tests.Unit.Repositories
{
    public class GenericRepositoryTests : IDisposable
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
        GenericRepository<PrivateMessage, long> repository;

        public GenericRepositoryTests()
        {
            context = new MessengerDataContext(options);

            context.Database.EnsureDeleted();
            context.AddRange(testData);
            context.SaveChanges();

            repository = new GenericRepository<PrivateMessage, long>(context);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange

            // Act
            var resultData = (await repository.GetAllAsync()).ToList();

            // Assert
            Assert.NotNull(resultData);
            Assert.Equal(testData.Count(), resultData.Count());
        }

        [Fact]
        public async Task GetAsync_ReturnsExistingEntity()
        {
            // Arrange
            long searchId = 2;

            // Act
            var resultEntity = await repository.GetAsync(searchId);

            // Assert
            Assert.NotNull(resultEntity);
            Assert.Equal(searchId, resultEntity.Id);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsDeletedEntity()
        {
            // Arrange
            long deleteEntityId = 3;

            // Act
            var resultEntity = await repository.DeleteAsync(deleteEntityId);
            context.SaveChanges();
            var resultEntityList = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(resultEntity);
            Assert.Equal(deleteEntityId, resultEntity.Id);

            Assert.Equal(testData.Count() - 1, resultEntityList.Count());
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedEntity()
        {
            // Arrange
            var updatedEntity = new PrivateMessage { Id = 1, SenderUserId = "user3", ReceiverUserId = "user4", SentTime = DateTime.Now, Text = "updated message" };

            // Act
            var resultEntity = await repository.UpdateAsync(updatedEntity.Id, updatedEntity);

            // Assert
            Assert.NotNull(resultEntity);
            Assert.Equal(resultEntity, updatedEntity);
        }

        [Fact]
        public async Task FilterAsync_PredicateOnly_ReturnsFilteredEntityList()
        {
            // Arrange
            string senderUserId = "user2";
            var expectedEntityList = testData.Where(pm => pm.SenderUserId == senderUserId).ToList();

            // Act
            var resultEntityList = (await repository.FilterAsync(pm => pm.SenderUserId == senderUserId)).ToList();
            bool allEntitesContain = resultEntityList.All(pm => pm.SenderUserId == senderUserId);

            // Assert
            Assert.NotNull(expectedEntityList);
            Assert.True(allEntitesContain);
            Assert.Equal(expectedEntityList.Count(), resultEntityList.Count());
        }

        [Fact]
        public async Task FilterAsync_ReturnsFilteredEntityList()
        {
            // Arrange
            string senderUserId = "user2";

            // Act
            var resultEntityList = await repository.FilterAsync(pm => pm.SenderUserId == senderUserId);

            // Assert
            Assert.NotEmpty(resultEntityList);
        }

        [Fact]
        public async Task FindAsync_ValueOnly_ReturnsFilteredEntityList()
        {
            // Arrange
            string searchValue = "ar";

            // Act
            var resultEntityList = await repository.FindAsync(searchValue);

            // Assert
            Assert.NotEmpty(resultEntityList);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
