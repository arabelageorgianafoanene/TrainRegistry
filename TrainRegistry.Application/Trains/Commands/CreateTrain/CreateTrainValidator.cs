using FluentValidation;

namespace TrainRegistry.Application.Trains.Commands.CreateTrain
{
    public class CreateTrainValidator: AbstractValidator<CreateTrainCommand>
    {
        public CreateTrainValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
            RuleFor(x => x.Speed).GreaterThan(0);
        }
    }
}
