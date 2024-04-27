using FluentValidation.TestHelper;
using Moq;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands.Validators;
using ReenbitMessenger.DataAccess.Models.Domain;
using ReenbitMessenger.DataAccess.Repositories;
using ReenbitMessenger.DataAccess.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.PrivateMessageServices.Commands.Validators
{
    public class EditPrivateMessageCommandValidatorTests
    {
        private Mock<IPrivateMessageRepository> _privateMessageRepositoryMock = new Mock<IPrivateMessageRepository>();

        private EditPrivateMessageCommandValidator _validator;

        [Fact]
        public async Task Validate_ValidCommand_ReturnsValid()
        {
            // Arrange
            _privateMessageRepositoryMock.Setup(pmr => pmr.GetAsync(It.IsAny<long>())).ReturnsAsync(new PrivateMessage());

            _validator = new EditPrivateMessageCommandValidator(_privateMessageRepositoryMock.Object);

            var testModel = new EditPrivateMessageCommand(2, "updatedText");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.MessageId);
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Text);
        }

        [Fact]
        public async Task Validate_IdLesserThanZero_ReturnsErrorWithMessage()
        {
            // Arrange
            _privateMessageRepositoryMock.Setup(pmr => pmr.GetAsync(It.IsAny<long>())).ReturnsAsync(new PrivateMessage());

            _validator = new EditPrivateMessageCommandValidator(_privateMessageRepositoryMock.Object);

            var testModel = new EditPrivateMessageCommand(-2, "updatedText");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.MessageId).WithErrorMessage("Message Id cannot be lesser than 0.");
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Text);
        }

        [Fact]
        public async Task Validate_MessageIsNull_ReturnsErrorWithMessage()
        {
            // Arrange
            PrivateMessage nullMessage = null;

            _privateMessageRepositoryMock.Setup(pmr => pmr.GetAsync(It.IsAny<long>())).ReturnsAsync(nullMessage);

            _validator = new EditPrivateMessageCommandValidator(_privateMessageRepositoryMock.Object);

            var testModel = new EditPrivateMessageCommand(2, "updatedText");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.MessageId).WithErrorMessage("Message must exist.");
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.Text);
        }

        [Fact]
        public async Task Validate_TextIsEmpty_ReturnsErrorWithMessage()
        {
            // Arrange
            _privateMessageRepositoryMock.Setup(pmr => pmr.GetAsync(It.IsAny<long>())).ReturnsAsync(new PrivateMessage());

            _validator = new EditPrivateMessageCommandValidator(_privateMessageRepositoryMock.Object);

            var testModel = new EditPrivateMessageCommand(2, "");

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.MessageId);
            result.ShouldHaveValidationErrorFor(cmd => cmd.Text).WithErrorMessage("New message text cannot be empty.");
        }
    }
}
