using Moq;
using ReenbitMessenger.AppServices.PrivateMessageServices.Queries;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.PrivateMessageServices.Queries
{
    public class GetPrivateChatQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IPrivateMessageRepository> _privateMessageRepositoryMock = new Mock<IPrivateMessageRepository>();

        [Fact]
        public async Task Handle_ValidQuery_ReturnsListOfPrivateMessages()
        {
            // Arrange
            _privateMessageRepositoryMock.Setup(pmr => pmr.GetPrivateChatAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<PrivateMessage>() { new PrivateMessage(), new PrivateMessage()});

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IPrivateMessageRepository>()).Returns(_privateMessageRepositoryMock.Object);

            var query = new GetPrivateChatQuery("user1", "user2");

            var handler = new GetPrivateChatQueryHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
