
using TrainEventContracts;
using TrainEventContracts.Common;
using TrainRegistry.Domain.Abstractions;
using TrainRegistry.Domain.Events;

namespace TrainRegistry.Infrastructure.Messaging
{
    public static class TrainEventMapper
    {
        public const int EventVersion = 1;
        public static (TrainEvent TrainEvent, string RoutingKey) ToContract(IDomainEvent domainEvent)
        {
            return domainEvent switch
            {
                TrainRegisteredEvent e => (new TrainRegistered
                {
                    TrainId = e.TrainId,
                    TrainName = e.TrainName,
                    CreatedTime = e.UpdatedTime,
                    TrainStatus = e.Status,
                    EventId = Guid.NewGuid(),
                    EventVersion = EventVersion,
                    CorrelationId = Guid.NewGuid()
                },
                TrainEventRoutingKeys.TrainRegistered), 

                TrainStatusChangedEvent e => (new TrainStatusUpdated
                {
                    TrainId = e.TrainId,
                    OldTrainStatus = e.OldStatus.Value,
                    NewTrainStatus = e.NewStatus.Value,
                    UpdatedTime = e.UpdatedTime
                }, TrainEventRoutingKeys.TrainStatusChanged),
                _ => throw new ArgumentException($"Unknown domain event type: {domainEvent.GetType().Name}")
            };
        }
    }
}
