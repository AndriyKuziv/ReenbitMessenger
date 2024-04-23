using Microsoft.AspNetCore.Http;
using Moq;
using ReenbitMessenger.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.API.Tests.Unit.Controllers
{
    public class ControllerHelperShould
    {
        private Mock<HttpContext> _httpContextMock = new Mock<HttpContext>();

        [Fact]
        public async Task GetUserId_ReturnsUserId()
        {
            // Arrange
            string expectedId = "userId";

            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, expectedId);
            var claimEntity = new ClaimsIdentity(new List<Claim> { userIdClaim });
            var claimsPrincipal = new ClaimsPrincipal(claimEntity);

            _httpContextMock.Setup(ctxt => ctxt.User).Returns(claimsPrincipal);

            // Act
            var resultId = await ControllerHelper.GetUserId(_httpContextMock.Object);

            // Assert
            Assert.Equal(resultId, expectedId);
        }

        [Fact]
        public async Task GetUserId_ReturnsNull_IfIdentityIsNull()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal();

            _httpContextMock.Setup(ctxt => ctxt.User).Returns(claimsPrincipal);

            // Act
            var resultId = await ControllerHelper.GetUserId(_httpContextMock.Object);

            // Assert
            Assert.Null(resultId);
        }
    }
}
