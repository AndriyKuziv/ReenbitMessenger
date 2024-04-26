using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Commands
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;

        public CreateUserCommandHandlerTests()
        {
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IUserRepository>()).Returns(_userRepositoryMock.Object);

            var query = new CreateUserCommand("user1", "user1@email.com", "password");

            var handler = new CreateUserCommandHandler(_unitOfWorkMock.Object, _userManagerMock.Object);

            var result = await handler.Handle(query);

            Assert.NotNull(result);
        }
    }
}
