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
            _groupChatRepositoryMock.Setup(cr => cr.RemoveUserFromGroupChatAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(new GroupChatMember() { Id = 5 });
            _groupChatRepositoryMock.Setup(cr => cr.GetGroupChatMembersAsync(It.IsAny<Guid>())).ReturnsAsync(new List<GroupChatMember> { new GroupChatMember(), new GroupChatMember() });
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new RemoveUsersFromGroupChatCommand(new Guid(), new List<string> { "user1", "user2" });

            var handler = new RemoveUsersFromGroupChatCommandHandler(_unitOfWorkMock.Object);

            var result = await handler.Handle(command);

            Assert.NotNull(result);
        }
    }
}
