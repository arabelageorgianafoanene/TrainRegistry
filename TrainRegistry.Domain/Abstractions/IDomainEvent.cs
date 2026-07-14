using MediatR;

namespace TrainRegistry.Domain.Abstractions
{
    public interface IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccuredOn { get; }
    }
}
