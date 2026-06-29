using ErrorOr;
using MediatR;
using TrainRegistry.Domain.ValueObjects;

namespace TrainRegistry.Application.Trains.Commands.UpdateTrainStatus
{
   public record class UpdateTrainStatusCommand(Guid TrainId, TrainStatus TrainStatus) : IRequest<ErrorOr<Updated>> { }
}
