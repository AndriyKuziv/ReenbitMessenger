using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Queries;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Queries
{
    public class GetFullGroupChatQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidQuery_ReturnsFullGroupChat()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(cr => cr.GetFullAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat());

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var query = new GetFullGroupChatQuery(new Guid());

            var handler = new GetFullGroupChatQueryHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.NotNull(result);
        }
    }
}
