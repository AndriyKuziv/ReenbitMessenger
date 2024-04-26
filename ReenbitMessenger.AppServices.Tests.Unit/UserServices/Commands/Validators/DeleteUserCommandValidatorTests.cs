using Moq;
using ReenbitMessenger.AppServices.UserServices.Commands.Validators;
using ReenbitMessenger.AppServices.UserServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity;

namespace ReenbitMessenger.AppServices.Tests.Unit.UserServices.Commands.Validators
{
    public class DeleteUserCommandValidatorTests
    {
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private DeleteUserCommandValidator _validator;

        [Fact]
        public async Task DeleteUserCommandValidator_ValidCommand_ReturnsValid()
        {
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());

            _validator = new DeleteUserCommandValidator(_userRepositoryMock.Object);

            var testModel = new DeleteUserCommand("user1");

            var result = await _validator.TestValidateAsync(testModel);

            result.ShouldNotHaveValidationErrorFor(cmd => cmd.UserId);
        }
    }
}
