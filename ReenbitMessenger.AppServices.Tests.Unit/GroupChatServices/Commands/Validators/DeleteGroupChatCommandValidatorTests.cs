
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators;
using ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands.Validators
{
    public class DeleteGroupChatCommandValidatorTests
    {
        private Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        private DeleteGroupChatCommandValidator _validator;

        [Fact]
        public async Task Validate_ValidCommand_ReturnValid()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat { Id = new Guid() });

            _groupChatRepositoryMock.Setup(repo => repo.IsInGroupChat(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            _validator = new DeleteGroupChatCommandValidator(_groupChatRepositoryMock.Object);

            var testModel = new DeleteGroupChatCommand(new Guid());

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
        }

        [Fact]
        public async Task Validate_WrongChatId_ReturnErrorWithMessage()
        {
            // Arrange
            GroupChat nullChat = null;
            _groupChatRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(nullChat);

            _groupChatRepositoryMock.Setup(repo => repo.IsInGroupChat(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);

            _validator = new DeleteGroupChatCommandValidator(_groupChatRepositoryMock.Object);

            var testModel = new DeleteGroupChatCommand(new Guid());

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.GroupChatId).WithErrorMessage("Group chat must exist.");
        }
    }
}
