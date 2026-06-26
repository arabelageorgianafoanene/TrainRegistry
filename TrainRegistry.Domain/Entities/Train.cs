using TrainRegistry.Domain.Abstractions;
using TrainRegistry.Domain.Events;
using TrainRegistry.Domain.Exceptions;
using TrainRegistry.Domain.ValueObjects;

namespace TrainRegistry.Domain.Entities
{
    public class Train: Entity
    {
        public  string Name { get; private set; }

        public TrainLength Length {  get; private set; }

        public  TrainSpeed Speed {  get; private set; }

        public TrainStatus TrainStatus { get; private set; }

        private Train() { }

        public static Train Create(string name, double length, double speed)
        {
            var train = new Train
            {
                Id = Guid.NewGuid(),
                Name = name,
                Length = new TrainLength(length),
                Speed = new TrainSpeed(speed),
                TrainStatus = TrainStatus.Active
            };

            train.RaiseDomainEvent(new TrainCreatedEvent(train.Id, train.Name));

            return train;
        }

        public void ChangedStatus(TrainStatus newStatus)
        {
            if(!TrainStatus.CanTransitionTo(newStatus))
            {
                throw new InvalidTrainStatusChangeException(TrainStatus.Value, newStatus.Value);
            }

            var oldStatus = TrainStatus;

            TrainStatus = newStatus;

            RaiseDomainEvent(new TrainStatusChangedEvent(Id, oldStatus, newStatus));
        }
    }
}
