using Moq;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.AppServices.PrivateMessageServices.Queries;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.PrivateMessageServices.Queries
{
    public class GetPrivateMessageQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IPrivateMessageRepository> _privateMessageRepositoryMock = new Mock<IPrivateMessageRepository>();

        [Fact]
        public async Task Handle_ValidQuery_ReturnsPrivateMessage()
        {
            // Arrange
            _privateMessageRepositoryMock.Setup(pmr => pmr.GetAsync(It.IsAny<long>())).ReturnsAsync(new PrivateMessage());

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IPrivateMessageRepository>()).Returns(_privateMessageRepositoryMock.Object);

            var query = new GetPrivateMessageQuery(5);

            var handler = new GetPrivateMessageQueryHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.NotNull(result);
        }
    }
}
