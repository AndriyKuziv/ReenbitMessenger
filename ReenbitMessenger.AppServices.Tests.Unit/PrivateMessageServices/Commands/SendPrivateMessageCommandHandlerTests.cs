using Moq;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using Xunit.Sdk;

namespace ReenbitMessenger.AppServices.Tests.Unit.PrivateMessageServices.Commands
{
    public class SendPrivateMessageCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IPrivateMessageRepository> _privateMessageRepositoryMock = new Mock<IPrivateMessageRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSentPrivateMessage()
        {
            // Arrange
            PrivateMessage testMessage = new PrivateMessage()
            {
                Id = 1,
                Text = "newMessage",
                SenderUserId = "user1",
                ReceiverUserId = "user2"
            };

            _privateMessageRepositoryMock.Setup(pmr => pmr.AddAsync(It.IsAny<PrivateMessage>())).ReturnsAsync(testMessage);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IPrivateMessageRepository>()).Returns(_privateMessageRepositoryMock.Object);

            var query = new SendPrivateMessageCommand("user1", "user2", "newMessage", null);

            var handler = new SendPrivateMessageCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testMessage.Text, result.Text);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsNull()
        {
            PrivateMessage nullMessage = null;
            // Arrange
            _privateMessageRepositoryMock.Setup(pmr => pmr.AddAsync(It.IsAny<PrivateMessage>())).ReturnsAsync(nullMessage);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IPrivateMessageRepository>()).Returns(_privateMessageRepositoryMock.Object);

            var query = new SendPrivateMessageCommand("user1", "user2", "text", null);

            var handler = new SendPrivateMessageCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.Null(result);
        }
    }
}
