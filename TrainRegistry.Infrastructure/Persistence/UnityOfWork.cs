
using TrainRegistry.Application.Abstractions;
using TrainRegistry.Domain.Abstractions;
using TrainRegistry.Infrastructure.Messaging;

namespace TrainRegistry.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TrainDbContext _context;
        private readonly IEventPublisher _publisher;

        public UnitOfWork(TrainDbContext context, IEventPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var domainEvents = _context.ChangeTracker
                .Entries<Entity>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            var result = await _context.SaveChangesAsync(ct);

            foreach (var domainEvent in domainEvents)
            {
                var (contractEvent, routingKey) = TrainEventMapper.ToContract(domainEvent);
                await _publisher.Publish(contractEvent, routingKey, ct);
            }

            ClearDomainEvents(domainEvents);

            return result;
        }

        private void ClearDomainEvents(IEnumerable<IDomainEvent> events)
        {
            var entities = _context.ChangeTracker.Entries<Entity>()
                .Select(e => e.Entity);
            foreach (var entity in entities)
                entity.ClearDomainEvents();
        }
    }
}
