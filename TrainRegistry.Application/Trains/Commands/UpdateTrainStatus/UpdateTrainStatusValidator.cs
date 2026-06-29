
using FluentValidation;
using TrainRegistry.Domain.ValueObjects;

namespace TrainRegistry.Application.Trains.Commands.UpdateTrainStatus
{
    public class UpdateTrainStatusValidator: AbstractValidator<UpdateTrainStatusCommand>
    {
        public UpdateTrainStatusValidator()
        {
            RuleFor(x => x.TrainId)
               .NotEmpty()
               .Must(x => x != Guid.Empty)
               .WithMessage("Train ID is required and it must be a valid Guid!");
            RuleFor(x => x.TrainStatus)
                .Must(status => TrainStatus.All.Contains(status))
                .WithMessage("Train status shoudl contain a valid train status! ");
        }
    }
}
