using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ReenbitMessenger.AppServices.AuthServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.AppServices.Tests.Unit.AuthServices
{
    public class LogInQueryHandlerTests
    {
        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IUserClaimsPrincipalFactory<IdentityUser>> _usrClaimsPrFaMock;

        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<SignInManager<IdentityUser>> _signInManagerMock;

        public LogInQueryHandlerTests()
        {
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _usrClaimsPrFaMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(_userManagerMock.Object, _httpContextAccessorMock.Object, _usrClaimsPrFaMock.Object, null, null, null, null);
        }

        [Fact]
        public async Task Handler_NotValidQuery_ReturnsNull()
        {
            // Arrange
            IdentityUser nullUser = null;

            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(nullUser);
            _signInManagerMock.Setup(um => um.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(new SignInResult());

            var loginQueryHandler = new LogInQueryHandler(_userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var loginResult = await loginQueryHandler.Handle(new LogInQuery("", ""));

            // Assert
            Assert.Null(loginResult);
        }
    }
}
