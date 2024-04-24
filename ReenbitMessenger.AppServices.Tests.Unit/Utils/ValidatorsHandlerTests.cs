using FluentValidation;
using Moq;
using ReenbitMessenger.AppServices.Commands.GroupChatCommands;
using ReenbitMessenger.AppServices.Utils;

namespace ReenbitMessenger.AppServices.Tests.Unit.Utils
{
    public class ValidatorsHandlerTests
    {
        private Mock<IValidatorFactory> validatorFactoryMock = new Mock<IValidatorFactory>();

        private class TestClass
        {
            public string Name { get; set; }
            public int Number {  get; set; }
            public bool IsNumberGreaterThanOrEqualZero { get; set; }
        }

        private class TestClassValidator : AbstractValidator<TestClass>
        {
            public TestClassValidator()
            {
                RuleFor(tc => tc.Name).NotEmpty();

                RuleFor(tc => new { tc.Number, tc.IsNumberGreaterThanOrEqualZero }).Must(tc =>
                {
                    return (tc.Number >= 0 && tc.IsNumberGreaterThanOrEqualZero) ||
                        (tc.Number < 0 && !tc.IsNumberGreaterThanOrEqualZero);
                });
            }
        }

        [Fact]
        public async Task ValidateAsync_ReturnsValidResult()
        {
            // Arrange
            Type type = typeof(ICommand);

            validatorFactoryMock.Setup(vf => vf.GetValidator<TestClass>()).Returns(new TestClassValidator());

            var testClassObject = new TestClass
            {
                Name = "Test",
                Number = 1,
                IsNumberGreaterThanOrEqualZero = true,
            };

            var validatorsHandler = new ValidatorsHandler(validatorFactoryMock.Object);

            // Act
            var result = await validatorsHandler.ValidateAsync(testClassObject);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidateAsync_ReturnsNotValidResult()
        {
            // Arrange
            Type type = typeof(ICommand);

            validatorFactoryMock.Setup(vf => vf.GetValidator<TestClass>()).Returns(new TestClassValidator());

            var testClassObject = new TestClass
            {
                Name = "Test",
                Number = 1,
                IsNumberGreaterThanOrEqualZero = false,
            };

            var validatorsHandler = new ValidatorsHandler(validatorFactoryMock.Object);

            // Act
            var result = await validatorsHandler.ValidateAsync(testClassObject);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
        }
    }
}
