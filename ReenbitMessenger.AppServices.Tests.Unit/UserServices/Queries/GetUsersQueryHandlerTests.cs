using Moq;
using ReenbitMessenger.AppServices.UserServices.Queries;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Queries
{
    public class GetUsersQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsFilteredUsers()
        {
            _userRepositoryMock.Setup(cr => cr.FindAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<IdentityUser>() { new IdentityUser(), new IdentityUser() });
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IUserRepository>()).Returns(_userRepositoryMock.Object);

            var query = new GetUsersQuery();
            var handler = new GetUsersQueryHandler(_unitOfWorkMock.Object);

            var result = await handler.Handle(query);

            Assert.NotNull(result);
        }
    }
}
