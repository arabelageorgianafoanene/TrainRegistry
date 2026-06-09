using FluentValidation;

namespace TrainRegistry.Application.Trains.Queries.GetTrainById
{
    public class GetTrainByIdValidator: AbstractValidator<GetTrainByIdQuery>
    {
        public GetTrainByIdValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Train is must not be empty");
        }
    }
}
