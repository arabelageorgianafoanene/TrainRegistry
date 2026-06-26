
using MediatR;
using Microsoft.Extensions.Logging;
using TrainRegistry.Domain.Events;

namespace TrainRegistry.Application.Trains.EventHandlers
{
    public class CreateTrainEventHandler : INotificationHandler<TrainCreatedEvent>
    {
        private readonly ILogger<CreateTrainEventHandler> _logger;

        public CreateTrainEventHandler(ILogger<CreateTrainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(TrainCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Train created with ID: {TrainId} and Name: {TrainName}", notification.TrainId, notification.TrainName);
            return Task.CompletedTask;
        }
    }
}
