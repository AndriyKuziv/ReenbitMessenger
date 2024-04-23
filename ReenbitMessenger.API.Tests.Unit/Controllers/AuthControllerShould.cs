using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReenbitMessenger.API.Controllers;
using ReenbitMessenger.AppServices;
using ReenbitMessenger.AppServices.Queries.Auth;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.API.Tests.Unit.Controllers
{
    public class AuthControllerShould
    {
        private readonly Mock<ITokenHandler> _tokenHandlerMock = new Mock<ITokenHandler>();
        private readonly Mock<IServiceProvider> _serviceProviderMock = new Mock<IServiceProvider>();
        private readonly Mock<HandlersDispatcher> _handlersDispatcherMock = new Mock<HandlersDispatcher>();

        private readonly Mock<IValidatorsHandler> _validatorsHandlerMock = new Mock<IValidatorsHandler>();

        [Fact]
        public async Task LogIn_ReturnsOkResult_WithToken()
        {
            // Arrange
            var loginRequest = new LoginRequest() { Username = "testUser", Password = "Test0=" };
            var existingUser = new Microsoft.AspNetCore.Identity.IdentityUser() { UserName = "testUser", Email = "test@gmail.com"};
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMDQ5NmI0NTAtMDIwZi00Y2Y3LThjMDEtMTBiNmIzY2ZjNTJlIiwiZXhwIjoxNzEzNzQzMjY3LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDUxIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA1MSJ9.AbS4kQAi63oGDGfm6bK1g4UbJ-i0bJHyeIwqx3lpkWQ";

            _handlersDispatcherMock.Setup(disp => disp.Dispatch(It.IsAny<LogInQuery>())).ReturnsAsync(existingUser);
            _tokenHandlerMock.Setup(han => han.CreateTokenAsync(It.IsAny<IdentityUser>())).ReturnsAsync(token);

            var authController = new AuthController(_handlersDispatcherMock.Object, _tokenHandlerMock.Object, _validatorsHandlerMock.Object);

            // Act
            var result = await authController.LogIn(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<string>(okResult.Value);
            Assert.NotNull(model);
        }
    }
}
