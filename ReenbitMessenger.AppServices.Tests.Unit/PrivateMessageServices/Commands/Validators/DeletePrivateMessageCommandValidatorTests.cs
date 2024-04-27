using Moq;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands.Validators;
using ReenbitMessenger.AppServices.PrivateMessageServices.Commands;
using ReenbitMessenger.DataAccess.Repositories;
using FluentValidation.TestHelper;
using ReenbitMessenger.DataAccess.Models.Domain;

namespace ReenbitMessenger.AppServices.Tests.Unit.PrivateMessageServices.Commands.Validators
{
    public class DeletePrivateMessageCommandValidatorTests
    {
        private Mock<IPrivateMessageRepository> _privateMessageRepositoryMock = new Mock<IPrivateMessageRepository>();

        private DeletePrivateMessageCommandValidator _validator;

        [Fact]
        public async Task Validate_ValidCommand_ReturnsValid()
        {
            // Arrange

            _privateMessageRepositoryMock.Setup(pmr => pmr.GetAsync(It.IsAny<long>())).ReturnsAsync(new PrivateMessage());

            _validator = new DeletePrivateMessageCommandValidator(_privateMessageRepositoryMock.Object);

            var testModel = new DeletePrivateMessageCommand(2);

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cmd => cmd.MessageId);
        }

        [Fact]
        public async Task Validate_IdLesserThanZero_ReturnsErrorWithMessage()
        {
            // Arrange
            _privateMessageRepositoryMock.Setup(pmr => pmr.GetAsync(It.IsAny<long>())).ReturnsAsync(new PrivateMessage());

            _validator = new DeletePrivateMessageCommandValidator(_privateMessageRepositoryMock.Object);

            var testModel = new DeletePrivateMessageCommand(-2);

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.MessageId).WithErrorMessage("Message Id cannot be lesser than 0.");
        }

        [Fact]
        public async Task Validate_MessageIsNull_ReturnsErrorWithMessage()
        {
            // Arrange

            PrivateMessage nullMessage = null;

            _privateMessageRepositoryMock.Setup(pmr => pmr.GetAsync(It.IsAny<long>())).ReturnsAsync(nullMessage);

            _validator = new DeletePrivateMessageCommandValidator(_privateMessageRepositoryMock.Object);

            var testModel = new DeletePrivateMessageCommand(-1);

            // Act
            var result = await _validator.TestValidateAsync(testModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.MessageId).WithErrorMessage("Message must exist.");
        }
    }
}
