using Moq;
using ReenbitMessenger.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.AppServices.UserServices.Commands.Validators;
using ReenbitMessenger.AppServices.UserServices.Commands;
using FluentValidation.TestHelper;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Commands.Validators
{
    public class CreateUserCommandValidatorTests
    {
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private CreateUserCommandValidator _validator;

        [Fact]
        public async Task CreateUserCommandValidator_ValidCommand_ReturnsValid()
        {
            _userRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _validator = new CreateUserCommandValidator(_userRepositoryMock.Object);

            var testModel = new CreateUserCommand("user1", "user1@email.com", "User2=");

            var result = await _validator.TestValidateAsync(testModel);

            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Username);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Email);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Password);
        }

        [Fact]
        public async Task CreateUserCommandValidator_NotValidCommand_ReturnsErrorsWithMessages()
        {
            _userRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _validator = new CreateUserCommandValidator(_userRepositoryMock.Object);

            var testModel = new CreateUserCommand("", "", "");

            var result = await _validator.TestValidateAsync(testModel);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Username).WithErrorMessage("User name cannot be empty.");
            result.ShouldHaveValidationErrorFor(cmd => cmd.Email).WithErrorMessage("Email address has been entered in wrong format.");
            result.ShouldHaveValidationErrorFor(cmd => cmd.Password).WithErrorMessage("Password cannot be empty.");
        }

        [Fact]
        public async Task CreateUserCommandValidator_NonUniqueEmail_ReturnsErrorWithMessage()
        {
            _userRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _validator = new CreateUserCommandValidator(_userRepositoryMock.Object);

            var testModel = new CreateUserCommand("user1", "user1@email.com", "User2=");

            var result = await _validator.TestValidateAsync(testModel);

            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Username);
            result.ShouldHaveValidationErrorFor(cmd => cmd.Email).WithErrorMessage("The email must be unique.");
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Password);

        }
    }
}
