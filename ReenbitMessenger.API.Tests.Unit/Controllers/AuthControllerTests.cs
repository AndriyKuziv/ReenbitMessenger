using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReenbitMessenger.API.Controllers;
using ReenbitMessenger.AppServices;
using ReenbitMessenger.AppServices.Commands.User;
using ReenbitMessenger.AppServices.Queries.Auth;
using ReenbitMessenger.AppServices.Utils;
using ReenbitMessenger.Infrastructure.Models.Requests;

namespace ReenbitMessenger.API.Tests.Unit.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<ITokenHandler> _tokenHandlerMock = new Mock<ITokenHandler>();
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<HandlersDispatcher> _handlersDispatcherMock;

        private readonly Mock<IValidatorsHandler> _validatorsHandlerMock = new Mock<IValidatorsHandler>();

        public AuthControllerTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _handlersDispatcherMock = new Mock<HandlersDispatcher>(_serviceProviderMock.Object);
        }

        [Fact]
        public async Task LogIn_ExistingUser_ReturnsOkResult_WithToken()
        {
            // Arrange
            var loginRequest = new Infrastructure.Models.Requests.LoginRequest() { Username = "testUser", Password = "Test0=" };
            var existingUser = new IdentityUser() { UserName = "testUser", Email = "test@gmail.com"};
            var token = "token";

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

        [Fact]
        public async Task LogIn_NonExistingUser_ReturnsNotFoundResult()
        {
            // Arrange
            var loginRequest = new Infrastructure.Models.Requests.LoginRequest() { Username = "testUser", Password = "Test0=" };
            IdentityUser nullUser = null;

            _handlersDispatcherMock.Setup(disp => disp.Dispatch(It.IsAny<LogInQuery>())).ReturnsAsync(nullUser);

            var authController = new AuthController(_handlersDispatcherMock.Object, _tokenHandlerMock.Object, _validatorsHandlerMock.Object);

            // Act
            var result = await authController.LogIn(loginRequest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SignUp_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var signupRequest = new CreateUserRequest() { Username = "testname", Email = "test@test.com", Password = "test" };
            Mock<ValidationResult> validationResultMock = new Mock<ValidationResult>();
            validationResultMock.Setup(validationResult => validationResult.IsValid).Returns(true);

            _validatorsHandlerMock.Setup(vh => vh.ValidateAsync(It.IsAny<CreateUserCommand>())).ReturnsAsync(validationResultMock.Object);
            _handlersDispatcherMock.Setup(disp => disp.Dispatch(It.IsAny<CreateUserCommand>())).ReturnsAsync(true);

            var authController = new AuthController(_handlersDispatcherMock.Object, _tokenHandlerMock.Object, _validatorsHandlerMock.Object);

            // Act
            var result = await authController.SignUp(signupRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SignUp_NotValidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var signupRequest = new CreateUserRequest() { Username = "testname", Email = "test@test.com", Password = "test" };
            Mock<ValidationResult> validationResultMock = new Mock<ValidationResult>();
            validationResultMock.Setup(validationResult => validationResult.IsValid).Returns(false);

            _validatorsHandlerMock.Setup(vh => vh.ValidateAsync(It.IsAny<CreateUserCommand>())).ReturnsAsync(validationResultMock.Object);

            var authController = new AuthController(_handlersDispatcherMock.Object, _tokenHandlerMock.Object, _validatorsHandlerMock.Object);

            // Act
            var result = await authController.SignUp(signupRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SignUp_CommandFailed_ReturnsBadRequestResult()
        {
            // Arrange
            var signupRequest = new CreateUserRequest() { Username = "testname", Email = "test@test.com", Password = "test" };
            Mock<ValidationResult> validationResultMock = new Mock<ValidationResult>();
            validationResultMock.Setup(validationResult => validationResult.IsValid).Returns(true);

            _validatorsHandlerMock.Setup(vh => vh.ValidateAsync(It.IsAny<CreateUserCommand>())).ReturnsAsync(validationResultMock.Object);
            _handlersDispatcherMock.Setup(disp => disp.Dispatch(It.IsAny<CreateUserCommand>())).ReturnsAsync(false);

            var authController = new AuthController(_handlersDispatcherMock.Object, _tokenHandlerMock.Object, _validatorsHandlerMock.Object);

            // Act
            var result = await authController.SignUp(signupRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
