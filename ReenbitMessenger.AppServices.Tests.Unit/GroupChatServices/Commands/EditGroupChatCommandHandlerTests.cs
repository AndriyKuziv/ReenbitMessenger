using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands
{
    public class EditGroupChatCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            _groupChatRepositoryMock.Setup(cr => cr.UpdateAsync(It.IsAny<Guid>(), It.IsAny<GroupChat>())).ReturnsAsync(new GroupChat() { Id = new Guid() });
            _unitOfWorkMock.Setup(uw => uw.GetRepository<IGroupChatRepository>()).Returns(_groupChatRepositoryMock.Object);

            var command = new EditGroupChatCommand(new Guid(), "newChatName");

            var handler = new EditGroupChatCommandHandler(_unitOfWorkMock.Object);

            var result = await handler.Handle(command);

            Assert.True(result);
        }
    }
}
