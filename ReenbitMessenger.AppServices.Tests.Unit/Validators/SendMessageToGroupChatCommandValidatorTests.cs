using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Tests.Unit.Validators
{
    public class SendMessageToGroupChatCommandValidatorTests
    {
        private Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        private SendMessageToGroupChatCommandValidator _validator;

        [Fact]
        public async Task SendMessageToGroupChatCommandValidator_ValidCommand_ReturnsValid()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat { Id = new Guid() });
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new IdentityUser { Id = userId });

            _validator = new SendMessageToGroupChatCommandValidator(_groupChatRepositoryMock.Object, _userRepositoryMock.Object);

            var testModel = new SendMessageToGroupChatCommand(new Guid(), "user1", "message");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.UserId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Text);
        }

        [Fact]
        public async Task SendMessageToGroupChatCommandValidator_NotValidCommand_ReturnsErrorsWithMessages()
        {
            // Arrange
            GroupChat nullChat = null;
            IdentityUser nullUser = null;
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(nullChat);
            _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(nullUser);

            _validator = new SendMessageToGroupChatCommandValidator(_groupChatRepositoryMock.Object, _userRepositoryMock.Object);

            var testModel = new SendMessageToGroupChatCommand(new Guid(), "user1", "");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.GroupChatId).WithErrorMessage("Group chat must exist.");
            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId).WithErrorMessage("Sender user must exist.");
            result.ShouldHaveValidationErrorFor(cmd => cmd.Text);
        }
    }
}
