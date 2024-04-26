using Moq;
using ReenbitMessenger.AppServices.GroupChatServices.Commands;
using ReenbitMessenger.AppServices.GroupChatServices.Commands.Validators;
using ReenbitMessenger.DataAccess.Repositories;
using FluentValidation.TestHelper;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.Tests.Unit.GroupChatServices.Commands.Validators
{
    public class DeleteMessageFromGroupChatCommandValidatorTests
    {
        private Mock<IGroupChatRepository> _groupChatRepositoryMock = new Mock<IGroupChatRepository>();

        private DeleteMessageFromGroupChatCommandValidator _validator;

        [Fact]
        public async Task DeleteMessageFromGroupChatCommandValidator_ValidCommand_ReturnsValid()
        {
            // Arrange
            _groupChatRepositoryMock.Setup(gcr => gcr.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new GroupChat());

            _groupChatRepositoryMock.Setup(gcr => gcr.GetMessageAsync(It.IsAny<long>())).ReturnsAsync(new GroupChatMessage());

            _validator = new DeleteMessageFromGroupChatCommandValidator(_groupChatRepositoryMock.Object);

            var testModel = new DeleteMessageFromGroupChatCommand(new Guid(), "user1", 5);

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.GroupChatId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.MessageId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.UserId);
        }

        [Fact]
        public async Task DeleteMessageFromGroupChatCommandValidator_NotValidCommand_ReturnsErrorsWithMessages()
        {
            // Arrange
            GroupChat nullChat = null;
            GroupChatMessage nullMessage = null;

            _groupChatRepositoryMock.Setup(gcr => gcr.GetAsync(It.IsAny<Guid>())).ReturnsAsync(nullChat);

            _groupChatRepositoryMock.Setup(gcr => gcr.GetMessageAsync(It.IsAny<long>())).ReturnsAsync(nullMessage);

            _validator = new DeleteMessageFromGroupChatCommandValidator(_groupChatRepositoryMock.Object);

            var testModel = new DeleteMessageFromGroupChatCommand(new Guid(), "", 5);

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.GroupChatId).WithErrorMessage("Group chat must exist.");
            result.ShouldHaveValidationErrorFor(cmd => cmd.MessageId).WithErrorMessage("The given message must exist.");
            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId);
        }
    }
}
