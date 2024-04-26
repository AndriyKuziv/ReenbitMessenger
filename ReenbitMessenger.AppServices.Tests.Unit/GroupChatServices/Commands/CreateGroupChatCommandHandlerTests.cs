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
            _groupChatRepositoryMock.Setup(cr => cr.AddAsync(It.IsAny<GroupChat>())).ReturnsAsync(new GroupChat() { Id = new Guid() }); 
            _groupChatRepositoryMock.Setup(cr => cr.AddUserToGroupChatAsync(It.IsAny<GroupChatMember>())).ReturnsAsync(new GroupChatMember() { Id = 5 });
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new CreateGroupChatCommand("chatName", "user1");

            var handler = new CreateGroupChatCommandHandler(_unitOfWorkMock.Object);

            var result = await handler.Handle(command);

            Assert.NotNull(result);
        }
    }
}
