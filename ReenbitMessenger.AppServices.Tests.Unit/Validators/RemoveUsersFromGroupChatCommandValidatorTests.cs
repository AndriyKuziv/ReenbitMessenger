using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Tests.Unit.Validators
{
    public class RemoveUsersFromGroupChatCommandValidatorTests
    {
        private Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private RemoveUsersFromGroupChatCommandValidator _validator;

        [Fact]
        public async Task RemoveUsersFromGroupChatCommandValidator_ValidCommand_ReturnsValid()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat { Id = new Guid() });
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new IdentityUser { Id = userId });

            _groupChatRepositoryMock.Setup(repo => repo.IsInGroupChat(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            _validator = new RemoveUsersFromGroupChatCommandValidator(_userRepositoryMock.Object, _groupChatRepositoryMock.Object);

            var testModel = new RemoveUsersFromGroupChatCommand(new Guid(), new List<string>() { "user1", "user2" });

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.UsersIds);
        }

        [Fact]
        public async Task RemoveUsersFromGroupChatCommandValidator_EmptyUsersList_ReturnsErrorWithMessage()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat { Id = new Guid() });
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser() { Id = "user1"});

            _groupChatRepositoryMock.Setup(repo => repo.IsInGroupChat(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

            _validator = new RemoveUsersFromGroupChatCommandValidator(_userRepositoryMock.Object, _groupChatRepositoryMock.Object);

            var testModel = new RemoveUsersFromGroupChatCommand(new Guid(), new List<string>() { });

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.UsersIds).WithErrorMessage("List of users ids cannot be empty.");
        }

        [Fact]
        public async Task RemoveUsersFromGroupChatCommandValidator_NotValidCommand_ReturnsErrorsWithMessages()
        {
            // Arrange
            GroupChat nullChat = null;
            IdentityUser nullUser = null;
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(nullChat);
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(nullUser);

            _groupChatRepositoryMock.Setup(repo => repo.IsInGroupChat(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            _validator = new RemoveUsersFromGroupChatCommandValidator(_userRepositoryMock.Object, _groupChatRepositoryMock.Object);

            var testModel = new RemoveUsersFromGroupChatCommand(new Guid(), new List<string>() { "user1", "user2" });

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.GroupChatId).WithErrorMessage("Group chat must exist.");
            result.ShouldHaveValidationErrorFor(cmd => cmd.UsersIds).WithErrorMessage("All users must exist.");
            result.ShouldHaveValidationErrorFor(cmd => new { cmd.GroupChatId, cmd.UsersIds }).WithErrorMessage("All of the given users must be members of this group chat.");
        }
    }
}
