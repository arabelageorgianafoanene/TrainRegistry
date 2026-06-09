using MediatR;

namespace TrainRegistry.Application.Trains.Commands.CreateTrain
{
    public record CreateTrainCommand(string Name, double Length, double Speed) : IRequest<Guid>;
}
