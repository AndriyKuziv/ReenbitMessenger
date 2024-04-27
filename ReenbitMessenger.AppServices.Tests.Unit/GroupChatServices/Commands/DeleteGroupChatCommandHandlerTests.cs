using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands
{
    public class DeleteGroupChatCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(cr => cr.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat() { Id = new Guid() });

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new DeleteGroupChatCommand(new Guid());

            var handler = new DeleteGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFalse()
        {
            // Arrange
            GroupChat nullChat = null;

            _groupChatRepositoryMock.Setup(cr => cr.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(nullChat);

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new DeleteGroupChatCommand(new Guid());

            var handler = new DeleteGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result);
        }
    }
}
