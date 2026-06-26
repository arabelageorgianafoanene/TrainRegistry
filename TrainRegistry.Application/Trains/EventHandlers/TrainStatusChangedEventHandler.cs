
using MediatR;
using Microsoft.Extensions.Logging;
using TrainRegistry.Domain.Events;

namespace TrainRegistry.Application.Trains.EventHandlers
{
    public class TrainStatusChangedEventHandler: INotificationHandler<TrainStatusChangedEvent>
    {
        private readonly ILogger<TrainStatusChangedEventHandler> _logger;

        public TrainStatusChangedEventHandler(ILogger<TrainStatusChangedEventHandler> logger)
        {
            _logger = logger;
        }


        public Task Handle(TrainStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Train status changed for Train ID: {TrainId} from {OldStatus} to {NewStatus}", notification.TrainId, notification.OldStatus.Value, notification.NewStatus.Value);

            return Task.CompletedTask;
        }
    }
}
