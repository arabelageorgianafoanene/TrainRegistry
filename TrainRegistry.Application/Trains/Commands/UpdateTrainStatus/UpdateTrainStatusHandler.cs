using ErrorOr;
using MediatR;
using TrainRegistry.Application.Interfaces;

namespace TrainRegistry.Application.Trains.Commands.UpdateTrainStatus
{
    public class UpdateTrainStatusHandler : IRequestHandler<UpdateTrainStatusCommand, ErrorOr<Updated>>
    {
        private readonly ITrainRepository _trainRepository;
        public UpdateTrainStatusHandler(ITrainRepository trainRepository)
        {
            _trainRepository = trainRepository;
        }
        public async Task<ErrorOr<Updated>> Handle(UpdateTrainStatusCommand updateTrainStatusRequest, CancellationToken cancellationToken)
        {
            var train = await _trainRepository.GetByIdAsync(updateTrainStatusRequest.TrainId, cancellationToken);

            if (train == null)
            {
                return Error.NotFound("Train.NotFound", $"Train with ID {updateTrainStatusRequest.TrainId} was not found.");
            }

            if (train.TrainStatus.CanTransitionTo(updateTrainStatusRequest.TrainStatus))
            {
                train.ChangedStatus(updateTrainStatusRequest.TrainStatus);
                await _trainRepository.UpdateAsync(train, cancellationToken);
                return Result.Updated;
            }

            return Error.Validation("Train.InvalidStatusTransition", $"Train cannot transit to the train status: {updateTrainStatusRequest.TrainStatus.Value}");                
        }
    }
}
