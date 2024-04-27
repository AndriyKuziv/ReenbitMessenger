using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands
{
    public class AddUsersToGroupChatCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsAddedUsers()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(cr => cr.AddUserToGroupChatAsync(It.IsAny<GroupChatMember>())).ReturnsAsync(new GroupChatMember() { Id = 5 });
            _groupChatRepositoryMock.Setup(cr => cr.FilterMembersAsync(It.IsAny<Func<GroupChatMember, bool>>())).ReturnsAsync(new List<GroupChatMember>() { new GroupChatMember(), new GroupChatMember() });
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new AddUsersToGroupChatCommand(new Guid(), new List<string>() { "user1", "user2" });
            var handler = new AddUsersToGroupChatCommandHandler(_unitOfWorkMock.Object);

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
            _groupChatRepositoryMock.Setup(cr => cr.AddUserToGroupChatAsync(It.IsAny<GroupChatMember>())).ReturnsAsync(nullMember);

            _groupChatRepositoryMock.Setup(cr => cr.FilterMembersAsync(It.IsAny<Func<GroupChatMember, bool>>())).ReturnsAsync(new List<GroupChatMember>() { new GroupChatMember(), new GroupChatMember() });

            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new AddUsersToGroupChatCommand(new Guid(), new List<string>() { "user1", "user2" });
            var handler = new AddUsersToGroupChatCommandHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Null(result);
        }
    }
}
