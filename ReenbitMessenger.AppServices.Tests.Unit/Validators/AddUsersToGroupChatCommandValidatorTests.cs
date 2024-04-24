using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands.Validators;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Tests.Unit.Validators
{
    public class AddUsersToGroupChatCommandValidatorTests
    {
        private Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private AddUsersToGroupChatCommandValidator _validator;

        [Fact]
        public async Task AddUsersToGroupChatCommandValidator_ValidCommand_ReturnsValid()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat { Id = new Guid() });
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new IdentityUser { Id = userId });

            _groupChatRepositoryMock.Setup(repo => repo.IsInGroupChat(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            _validator = new AddUsersToGroupChatCommandValidator(_groupChatRepositoryMock.Object, _userRepositoryMock.Object);

            var testModel = new AddUsersToGroupChatCommand(new Guid(), new List<string>() { "user1", "user2" });

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.UsersIds);
            result.ShouldNotHaveValidationErrorFor(cmd => new { cmd.GroupChatId, cmd.UsersIds });
        }

        [Fact]
        public async Task AddUsersToGroupChatCommandValidator_EmptyUsersList_ReturnsErrorWithMessage()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat { Id = new Guid() });
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new IdentityUser { Id = userId });

            _groupChatRepositoryMock.Setup(repo => repo.IsInGroupChat(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            _validator = new AddUsersToGroupChatCommandValidator(_groupChatRepositoryMock.Object, _userRepositoryMock.Object);

            var testModel = new AddUsersToGroupChatCommand(new Guid(), new List<string>() { });

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.UsersIds).WithErrorMessage("List of users to add cannot be empty.");
            result.ShouldNotHaveValidationErrorFor(cmd => new { cmd.GroupChatId, cmd.UsersIds });
        }

        [Fact]
        public async Task AddUsersToGroupChatCommandValidator_NotValid_ReturnsErrorsWithMessages()
        {
            // Arrange
            GroupChat nullChat = null;
            IdentityUser nullUser = null;
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(nullChat);
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(nullUser);

            _groupChatRepositoryMock.Setup(repo => repo.IsInGroupChat(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

            _validator = new AddUsersToGroupChatCommandValidator(_groupChatRepositoryMock.Object, _userRepositoryMock.Object);

            var testModel = new AddUsersToGroupChatCommand(new Guid(), new List<string>() { "user1", "user2" });

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.GroupChatId).WithErrorMessage("Group chat must exist.");
            result.ShouldHaveValidationErrorFor(cmd => cmd.UsersIds).WithErrorMessage("All of the users ids must exist.");
            result.ShouldHaveValidationErrorFor(cmd => new { cmd.GroupChatId, cmd.UsersIds }).WithErrorMessage("All of the given users must not be members of this group chat.");
        }
    }
}
