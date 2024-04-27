
using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Commands
{
    public class EditUserInfoCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;

        public EditUserInfoCommandHandlerTests()
        {
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.UpdateAsync(It.IsAny<string>(), It.IsAny<IdentityUser>())).ReturnsAsync(new IdentityUser());
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IUserRepository>()).Returns(_userRepositoryMock.Object);

            var query = new EditUserInfoCommand("user1", "newUsername", "newEmail@email.com");

            var handler = new EditUserInfoCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.True(result);
        }
    }
}
