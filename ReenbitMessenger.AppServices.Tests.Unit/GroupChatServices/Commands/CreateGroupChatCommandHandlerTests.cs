using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands
{
    public class CreateGroupChatCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsCreatedGroup()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(cr => cr.AddAsync(It.IsAny<GroupChat>())).ReturnsAsync(new GroupChat() { Id = new Guid() });

            _groupChatRepositoryMock.Setup(cr => cr.AddUserToGroupChatAsync(It.IsAny<GroupChatMember>())).ReturnsAsync(new GroupChatMember() { Id = 5 });

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new CreateGroupChatCommand("chatName", "user1");

            var handler = new CreateGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Handler_NullChat_ReturnsNull()
        {
            // Arrange
            GroupChat nullChat = null;
            _groupChatRepositoryMock.Setup(cr => cr.AddAsync(It.IsAny<GroupChat>())).ReturnsAsync(nullChat);

            _groupChatRepositoryMock.Setup(cr => cr.AddUserToGroupChatAsync(It.IsAny<GroupChatMember>())).ReturnsAsync(new GroupChatMember() { Id = 5 });

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new CreateGroupChatCommand("chatName", "user1");

            var handler = new CreateGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handler_NullChatMember_ReturnsNull()
        {
            // Arrange
            GroupChatMember nullMember = null;
            _groupChatRepositoryMock.Setup(cr => cr.AddAsync(It.IsAny<GroupChat>())).ReturnsAsync(new GroupChat() { Id = new Guid() });

            _groupChatRepositoryMock.Setup(cr => cr.AddUserToGroupChatAsync(It.IsAny<GroupChatMember>())).ReturnsAsync(nullMember);

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new CreateGroupChatCommand("chatName", "user1");

            var handler = new CreateGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Null(result);
        }
    }
}
