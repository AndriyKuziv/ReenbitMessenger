using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands
{
    public class SendMessageToGroupChatCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSentMessage()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(cr => cr.CreateGroupChatMessageAsync(It.IsAny<GroupChatMessage>())).ReturnsAsync(new GroupChatMessage() { Id = 5 });

            _groupChatRepositoryMock.Setup(cr => cr.GetMessageAsync(It.IsAny<long>())).ReturnsAsync(new GroupChatMessage());

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new SendMessageToGroupChatCommand(new Guid(), "user1", "text");

            var handler = new SendMessageToGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsNull()
        {
            // Arrange
            GroupChatMessage nullMessage = null;

            _groupChatRepositoryMock.Setup(cr => cr.CreateGroupChatMessageAsync(It.IsAny<GroupChatMessage>())).ReturnsAsync(nullMessage);

            _groupChatRepositoryMock.Setup(cr => cr.GetMessageAsync(It.IsAny<long>())).ReturnsAsync(new GroupChatMessage());

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new SendMessageToGroupChatCommand(new Guid(), "user1", "text");

            var handler = new SendMessageToGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Null(result);
        }
    }
}
