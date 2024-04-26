using Moq;
using ReenbitMessenger.AppServices.UserServices.Queries;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Queries
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsUser()
        {
            _userRepositoryMock.Setup(cr => cr.GetAsync(It.IsAny<string>())).ReturnsAsync(new Microsoft.AspNetCore.Identity.IdentityUser());
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IUserRepository>()).Returns(_userRepositoryMock.Object);

            var query = new GetUserByIdQuery("user1");

            var handler = new GetUserByIdQueryHandler(_unitOfWorkMock.Object);

            var result = await handler.Handle(query);

            Assert.NotNull(result);
        }
    }
}
