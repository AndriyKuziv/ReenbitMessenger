using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Commands
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            _userRepositoryMock.Setup(ur => ur.DeleteAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IUserRepository>()).Returns(_userRepositoryMock.Object);

            var query = new DeleteUserCommand("user1");

            var handler = new DeleteUserCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.True(result);
        }
    }
}
