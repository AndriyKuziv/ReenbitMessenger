using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands
{
    public class DeleteMessageFromGroupChatCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsDeletedMessage()
        {
            _groupChatRepositoryMock.Setup(cr => cr.GetMessageAsync(It.IsAny<long>())).ReturnsAsync(new GroupChatMessage() { Id = 5, SenderUserId = "user1" });
            _groupChatRepositoryMock.Setup(cr => cr.DeleteGroupChatMessageAsync(It.IsAny<Guid>(), It.IsAny<long>())).ReturnsAsync(new GroupChatMessage() { Id = 5 });
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new DeleteMessageFromGroupChatCommand(new Guid(), "user1", 5);

            var handler = new DeleteMessageFromGroupChatCommandHandler(_unitOfWorkMock.Object);

            var result = await handler.Handle(command);

            Assert.NotNull(result);
        }
    }
}
