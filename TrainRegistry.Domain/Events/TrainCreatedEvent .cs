using TrainRegistry.Domain.Abstractions;

namespace TrainRegistry.Domain.Events
{
    public class TrainRegisteredEvent : IDomainEvent
    {
        public Guid Id {  get; }

        public DateTime UpdatedTime { get; }

        public Guid TrainId { get; }

        public string TrainName { get; }

        public string Status { get; }

        public TrainRegisteredEvent(Guid trainId, string trainName, string status)
        {
            Id = Guid.NewGuid();
            UpdatedTime = DateTime.UtcNow;
            TrainId = trainId;
            TrainName = trainName;
            Status = status;
        }
    }
}
