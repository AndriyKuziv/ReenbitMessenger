using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.UserServices.Queries;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Queries
{
    public class GetUserByUsernameQueryHandlerTests
    {
        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;

        public GetUserByUsernameQueryHandlerTests()
        {
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsUser()
        {
            // Arrange
            _userManagerMock.Setup(cr => cr.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser());

            var query = new GetUserByUsernameQuery("user1");

            var handler = new GetUserByUsernameQueryHandler(_userManagerMock.Object);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.NotNull(result);
        }
    }
}
