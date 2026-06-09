using TrainRegistry.Application.Trains.Commands.CreateTrain;

namespace TrainRegistry.Application.Tests.Validators
{
    public class CreateTrainValidatorTests
    {
        private readonly CreateTrainValidator _createTrainValidator;

        public CreateTrainValidatorTests()
        {
            _createTrainValidator = new CreateTrainValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateTrainCommand(string.Empty, default, default);
            var result = _createTrainValidator.Validate(command);
            Assert.False(result.IsValid);

            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5.1)]
        public void Should_Have_Error_When_Length_Is_Negative_Or_Zero(double invalidLength)
        {
            var command = new CreateTrainCommand("name", invalidLength, default);

            var result = _createTrainValidator.Validate(command);

            Assert.False(result.IsValid);

            Assert.Contains(result.Errors, e => e.PropertyName == "Length");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1.5)]
        public void Should_Have_Errors_When_Speed_Is_Negative_Or_Zero(double invalidSpeed)
        {
            var command = new CreateTrainCommand("name", default, invalidSpeed);

            var result = _createTrainValidator.Validate(command);

            Assert.False(result.IsValid);

            Assert.Contains(result.Errors, e=>e.PropertyName == "Speed");
        }
    }
}