using Moq;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.PrivateMessageServices.Commands
{
    public class EditPrivateMessageCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IPrivateMessageRepository> _privateMessageRepositoryMock = new Mock<IPrivateMessageRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsEditedPrivateMessage()
        {
            // Arrange
            string updatedText = "updatedsText";
            _privateMessageRepositoryMock.Setup(pmr => pmr.UpdateAsync(It.IsAny<long>(), It.IsAny<PrivateMessage>())).ReturnsAsync(new PrivateMessage() { Id = 1, Text = updatedText});

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IPrivateMessageRepository>()).Returns(_privateMessageRepositoryMock.Object);

            var query = new EditPrivateMessageCommand(0, updatedText);

            var handler = new EditPrivateMessageCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Text);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsNull()
        {
            // Arrange
            string updatedText = "updatedsText";
            PrivateMessage nullMessage = null;
            _privateMessageRepositoryMock.Setup(pmr => pmr.UpdateAsync(It.IsAny<long>(), It.IsAny<PrivateMessage>())).ReturnsAsync(nullMessage);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IPrivateMessageRepository>()).Returns(_privateMessageRepositoryMock.Object);

            var query = new EditPrivateMessageCommand(0, updatedText);

            var handler = new EditPrivateMessageCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.Null(result);
        }
    }
}
