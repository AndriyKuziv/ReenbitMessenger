using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands.Validators
{
    public class EditGroupChatCommandValidatorTests
    {
        private Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        private EditGroupChatCommandValidator _validator;

        [Fact]
        public async Task Validate_ValidCommand_ReturnsValid()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat { Id = new Guid() });

            _validator = new EditGroupChatCommandValidator(_groupChatRepositoryMock.Object);

            var testModel = new EditGroupChatCommand(new Guid(), "newChatName");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Name);
        }

        [Fact]
        public async Task Validate_WrongChatId_ReturnsErrorWithMessage()
        {
            // Arrange
            GroupChat nullChat = null;
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(nullChat);

            _validator = new EditGroupChatCommandValidator(_groupChatRepositoryMock.Object);

            var testModel = new EditGroupChatCommand(new Guid(), "newChatName");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.GroupChatId).WithErrorMessage("Group chat must exist.");
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Name);
        }

        [Fact]
        public async Task Validate_WrongChatName_ReturnsError()
        {
            // Arrange
            GroupChat nullChat = null;
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat { Id = new Guid() });

            _validator = new EditGroupChatCommandValidator(_groupChatRepositoryMock.Object);

            var testModel = new EditGroupChatCommand(new Guid(), "");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.Name);
        }
    }
}
