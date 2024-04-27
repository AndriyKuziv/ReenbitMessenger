using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands
{
    public class RemoveUsersFromGroupChatCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsRemovedUsersIds()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(cr => cr.RemoveUserFromGroupChatAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(new GroupChatMember() { Id = 5 });

            _groupChatRepositoryMock.Setup(cr => cr.GetGroupChatMembersAsync(It.IsAny<Guid>())).ReturnsAsync(new List<GroupChatMember> { new GroupChatMember(), new GroupChatMember() });

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new RemoveUsersFromGroupChatCommand(new Guid(), new List<string> { "user1", "user2" });

            var handler = new RemoveUsersFromGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsNull()
        {
            // Arrange
            GroupChatMember nullMember = null;

            _groupChatRepositoryMock.Setup(cr => cr.RemoveUserFromGroupChatAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(nullMember);

            _groupChatRepositoryMock.Setup(cr => cr.GetGroupChatMembersAsync(It.IsAny<Guid>())).ReturnsAsync(new List<GroupChatMember> { new GroupChatMember(), new GroupChatMember() });

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new RemoveUsersFromGroupChatCommand(new Guid(), new List<string> { "user1", "user2" });

            var handler = new RemoveUsersFromGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Null(result);
        }
    }
}
