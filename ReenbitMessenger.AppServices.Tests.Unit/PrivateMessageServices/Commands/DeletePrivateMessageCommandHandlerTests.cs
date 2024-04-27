using Moq;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.PrivateMessageServices.Commands
{
    public class DeletePrivateMessageCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IPrivateMessageRepository> _privateMessageRepositoryMock = new Mock<IPrivateMessageRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsDeletedPrivateMessage()
        {
            // Arrange
            _privateMessageRepositoryMock.Setup(pmr => pmr.DeleteAsync(It.IsAny<long>())).ReturnsAsync(new PrivateMessage() { Id = 1 });

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IPrivateMessageRepository>()).Returns(_privateMessageRepositoryMock.Object);

            var query = new DeletePrivateMessageCommand(1);

            var handler = new DeletePrivateMessageCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsNull()
        {
            // Arrange
            PrivateMessage nullMessage = null;
            _privateMessageRepositoryMock.Setup(pmr => pmr.UpdateAsync(It.IsAny<long>(), It.IsAny<PrivateMessage>())).ReturnsAsync(nullMessage);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IPrivateMessageRepository>()).Returns(_privateMessageRepositoryMock.Object);

            var query = new DeletePrivateMessageCommand(0);

            var handler = new DeletePrivateMessageCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.Null(result);
        }
    }
}
