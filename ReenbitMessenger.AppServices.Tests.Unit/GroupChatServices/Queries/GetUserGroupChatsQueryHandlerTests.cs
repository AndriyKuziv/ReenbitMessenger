using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Queries;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Queries
{
    public class GetUserGroupChatsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsUserGroupChats()
        {
            _groupChatRepositoryMock.Setup(cr => cr.GetUserChatsAsync(It.IsAny<string>())).ReturnsAsync(new List<GroupChat>() { new GroupChat(), new GroupChat() });
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var query = new GetUserGroupChatsQuery("user1");

            var handler = new GetUserGroupChatsQueryHandler(_unitOfWorkMock.Object);

            var result = await handler.Handle(query);

            Assert.NotNull(result);
        }
    }
}
