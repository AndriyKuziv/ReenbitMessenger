using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators;
using ReenbitMessenger.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.AppServices.Tests.Unit.Validators
{
    public class CreateGroupChatCommandValidatorTests
    {
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private CreateGroupChatCommandValidator _validator;

        [Fact]
        public async Task CreateGroupChatCommandValidator_ValidCommand_ReturnsValid()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new IdentityUser { Id = userId });

            _validator = new CreateGroupChatCommandValidator(_userRepositoryMock.Object);

            var testModel = new CreateGroupChatCommand("chatName", "user1");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Name);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.UserId);
        }

        [Fact]
        public async Task CreateGroupChatCommandValidator_EmptyUserId_ReturnsError()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new IdentityUser { Id = userId });

            _validator = new CreateGroupChatCommandValidator(_userRepositoryMock.Object);

            var testModel = new CreateGroupChatCommand("chatName", "");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Name);
            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId);
        }

        [Fact]
        public async Task CreateGroupChatCommandValidator_EmptyChatName_ReturnsError()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new IdentityUser { Id = userId });

            _validator = new CreateGroupChatCommandValidator(_userRepositoryMock.Object);

            var testModel = new CreateGroupChatCommand("", "user1");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.Name);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.UserId);
        }

        [Fact]
        public async Task CreateGroupChatCommandValidator_WrongUserId_ReturnsErrorWithMessage()
        {
            // Arrange
            IdentityUser nullUser = null;
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(nullUser);

            _validator = new CreateGroupChatCommandValidator(_userRepositoryMock.Object);

            var testModel = new CreateGroupChatCommand("chat1", "user1");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Name);
            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId).WithErrorMessage("User must exist.");
        }
    }
}
