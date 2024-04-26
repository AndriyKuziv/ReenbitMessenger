using Moq;
using ReenbitMessenger.AppServices.UserServices.Commands.Validators;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;
using FluentValidation.TestHelper;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Commands.Validators
{
    public class EditUserInfoCommandValidatorTests
    {
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private EditUserInfoCommandValidator _validator;

        [Fact]
        public async Task EditUserCommandValidator_ValidCommand_ReturnsValid()
        {
            _userRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>())).ReturnsAsync(new Microsoft.AspNetCore.Identity.IdentityUser());

            _validator = new EditUserInfoCommandValidator(_userRepositoryMock.Object);

            var testModel = new EditUserInfoCommand("userId1", "user1", "user1@email.com");

            var result = await _validator.TestValidateAsync(testModel);

            result.ShouldNotHaveValidationErrorFor(cmd => cmd.UserId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Username);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Email);
        }
    }
}
