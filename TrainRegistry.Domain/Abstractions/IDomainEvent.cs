using MediatR;

namespace TrainRegistry.Domain.Abstractions
{
    public interface IDomainEvent: INotification
    {
        public Guid Id { get; }
        public DateTime OccuredOn { get; }
    }
}
