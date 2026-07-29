
using TrainEventContracts;
using TrainRegistry.Domain.Abstractions;
using TrainRegistry.Domain.Events;

namespace TrainRegistry.Infrastructure.Mappers
{
    public static class IntegrationEventMapper
    {
        public static TrainEventContracts.Common.TrainEvent Map(IDomainEvent domainEvent) =>
            domainEvent switch
            {
                TrainRegisteredEvent e => new TrainRegistered
                {
                    TrainName = e.TrainName,
                    TrainId = e.TrainId,
                    TrainStatus = e.Status,
                    CreatedTime = e.UpdatedTime
                },
                TrainStatusChangedEvent e => new TrainStatusUpdated
                {
                    TrainId = e.TrainId,
                    NewTrainStatus = e.NewStatus.Value,
                    OldTrainStatus = e.OldStatus.Value,
                    TrainName = e.TrainName
                },
                _ => throw new NotImplementedException()
            };
    }
}
