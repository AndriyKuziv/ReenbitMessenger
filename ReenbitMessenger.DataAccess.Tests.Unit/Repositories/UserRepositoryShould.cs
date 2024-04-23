using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.DataAccess.Tests.Unit.Repositories
{
    public class UserRepositoryShould : IDisposable
    {
        IQueryable<IdentityUser> testData = new List<IdentityUser>()
            {
                new IdentityUser(){ Email = "email1@test.com", UserName = "user1" },
                new IdentityUser(){ Email = "email2@test.com", UserName = "user2" },
                new IdentityUser(){ Email = "email3@test.com", UserName = "user3" },
                new IdentityUser(){ Email = "email4@test.com", UserName = "user4" },
            }.AsQueryable();

        DbContextOptions<MessengerDataContext> options = new DbContextOptionsBuilder<MessengerDataContext>()
            .UseInMemoryDatabase(databaseName: "IdentityUser")
            .Options;

        MessengerDataContext context;

        public UserRepositoryShould()
        {
            context = new MessengerDataContext(options);

            context.Database.EnsureDeleted();
            context.AddRange(testData);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // Arrange
            var repository = new UserRepository(context);

            // Act
            var resultUserList = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(resultUserList);
            Assert.Equal(testData.Count(), resultUserList.Count());
        }

        [Fact]
        public async Task IsEmailUnique_ReturnsTrueIfUnique()
        {
            // Arrange
            var repository = new UserRepository(context);
            string email = "uniqueEmail@test.com";

            // Act
            var isUniqueResult = await repository.IsEmailUniqueAsync(email);

            // Assert
            Assert.True(isUniqueResult);
        }

        [Fact]
        public async Task IsEmailUnique_ReturnsFalseIfNotUnique()
        {
            // Arrange
            var repository = new UserRepository(context);
            string email = "email1@test.com";

            // Act
            var isUniqueResult = await repository.IsEmailUniqueAsync(email);

            // Assert
            Assert.False(isUniqueResult);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedUser()
        {
            // Arrange
            var repository = new UserRepository(context);
            IdentityUser originalUser = context.Users.FirstOrDefault();
            IdentityUser updatedUser = new IdentityUser() { Email = "updated@test.com", UserName = "updatedName" };

            // Act
            var resultUser = await repository.UpdateAsync(originalUser.Id, updatedUser);

            // Assert
            Assert.NotNull(resultUser);
            Assert.NotEqual(resultUser, updatedUser);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
