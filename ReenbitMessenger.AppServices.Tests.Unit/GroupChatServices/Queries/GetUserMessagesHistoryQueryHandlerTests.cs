using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Queries;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Queries
{
    public class GetUserMessagesHistoryQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsFullGroupChat()
        {
            _groupChatRepositoryMock.Setup(cr => cr.GetMessageHistoryAsync(It.IsAny<string>())).ReturnsAsync(new List<GroupChatMessage>() { new GroupChatMessage(), new GroupChatMessage() });
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var query = new GetUserMessagesHistoryQuery("user1");

            var handler = new GetUserMessagesHistoryQueryHandler(_unitOfWorkMock.Object);

            var result = await handler.Handle(query);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
