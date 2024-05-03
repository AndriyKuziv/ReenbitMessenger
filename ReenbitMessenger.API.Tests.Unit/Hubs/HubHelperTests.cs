
using Microsoft.AspNetCore.SignalR;
using Moq;
using ReenbitMessenger.API.Controllers;
using ReenbitMessenger.API.Hubs;
using System.Security.Claims;

namespace ReenbitMessenger.API.Tests.Unit.Hubs
{
    public class HubHelperTests
    {
        private Mock<HubCallerContext> _contextMock = new Mock<HubCallerContext>();

        [Fact]
        public async Task GetUserId_ValidUser_ReturnsUserId()
        {
            string expectedId = "userId";

            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, expectedId);
            var claimEntity = new ClaimsIdentity(new List<Claim> { userIdClaim });
            var claimsPrincipal = new ClaimsPrincipal(claimEntity);

            _contextMock.Setup(ctxt => ctxt.User).Returns(claimsPrincipal);

            var resultId = await HubHelper.GetUserIdAsync(_contextMock.Object);

            Assert.NotNull(resultId);
            Assert.Equal(expectedId, resultId);
        }
    }
}
