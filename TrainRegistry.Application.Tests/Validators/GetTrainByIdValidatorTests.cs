
using TrainRegistry.Application.Trains.Queries.GetTrainById;

namespace TrainRegistry.Application.Tests.Validators
{
    public class GetTrainByIdValidatorTests
    {
        public readonly GetTrainByIdValidator _getTrainByIdValidator;

        public GetTrainByIdValidatorTests()
        {
            _getTrainByIdValidator = new GetTrainByIdValidator();
        }

        [Fact]
        public void Should_Have_Errors_When_Id_Is_Empty()
        {

            var query = new GetTrainByIdQuery(Guid.Empty);

            var result = _getTrainByIdValidator.Validate(query);

            Assert.False(result.IsValid);

            Assert.Contains(result.Errors, e => e.PropertyName == "Id");
        }
    }
}
