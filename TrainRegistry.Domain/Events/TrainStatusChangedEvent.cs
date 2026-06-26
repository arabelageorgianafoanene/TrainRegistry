using TrainRegistry.Domain.Abstractions;
using TrainRegistry.Domain.ValueObjects;

namespace TrainRegistry.Domain.Events
{
    public class TrainStatusChangedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public DateTime OccuredOn { get; }

        public Guid TrainId { get; }

        public TrainStatus OldStatus { get; }

        public TrainStatus NewStatus { get; }

        public TrainStatusChangedEvent(Guid trainId, TrainStatus oldStatus, TrainStatus newStatus)
        {
            Id = Guid.NewGuid();
            OccuredOn = DateTime.UtcNow;
            TrainId = trainId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
