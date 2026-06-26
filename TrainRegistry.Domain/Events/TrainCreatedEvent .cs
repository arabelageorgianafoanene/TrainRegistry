using TrainRegistry.Domain.Abstractions;

namespace TrainRegistry.Domain.Events
{
    public class TrainCreatedEvent : IDomainEvent
    {
        public Guid Id {  get; }

        public DateTime OccuredOn { get; }

        public Guid TrainId { get; }

        public string TrainName { get; }

        public TrainCreatedEvent(Guid trainId, string trainName)
        {
            Id = Guid.NewGuid();
            OccuredOn = DateTime.UtcNow;
            TrainId = trainId;
            TrainName = trainName;
        }
    }
}
