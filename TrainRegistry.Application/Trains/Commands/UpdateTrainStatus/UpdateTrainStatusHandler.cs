using ErrorOr;
using MediatR;
using TrainRegistry.Application.Abstractions;

namespace TrainRegistry.Application.Trains.Commands.UpdateTrainStatus
{
    public class UpdateTrainStatusHandler : IRequestHandler<UpdateTrainStatusCommand, ErrorOr<Updated>>
    {
        private readonly ITrainRepository _trainRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTrainStatusHandler(ITrainRepository trainRepository, IUnitOfWork unitOfWork)
        {
            _trainRepository = trainRepository;
            _unitOfWork = unitOfWork;
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
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Updated;
            }

            return Error.Validation("Train.InvalidStatusTransition", $"Train cannot transit to the train status: {updateTrainStatusRequest.TrainStatus.Value}");                
        }
    }
}
