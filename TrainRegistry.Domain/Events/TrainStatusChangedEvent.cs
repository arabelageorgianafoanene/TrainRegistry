using TrainRegistry.Domain.Abstractions;
using TrainRegistry.Domain.ValueObjects;

namespace TrainRegistry.Domain.Events
{
    public class TrainStatusChangedEvent : IDomainEvent
    {
        public Guid Id { get; }

        public DateTime UpdatedTime { get; }

        public Guid TrainId { get; }

        public string TrainName { get; }

        public TrainStatus OldStatus { get; }

        public TrainStatus NewStatus { get; }

        public TrainStatusChangedEvent(Guid trainId, TrainStatus oldStatus, TrainStatus newStatus, string name)
        {
            Id = Guid.NewGuid();
            UpdatedTime = DateTime.UtcNow;
            TrainId = trainId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            TrainName = name;
        }
    }
}
