using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TrainRegistry.Domain.Abstractions;

namespace TrainRegistry.Infrastructure.Persistence.Interceptors
{
    public class DomainEventsInterceptors: SaveChangesInterceptor
    {
        private readonly IPublisher _publisher;

        public DomainEventsInterceptors(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public override async ValueTask<int> SavedChangesAsync(
           SaveChangesCompletedEventData eventData,
           int result,
           CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context is null) return await base.SavedChangesAsync(eventData, result, cancellationToken);

            var entitiesWithEvents = context.ChangeTracker
                .Entries<Entity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToArray();

                entity.ClearDomainEvents();
                
                foreach (var domainEvent in events)
                {
                    await _publisher.Publish(domainEvent, cancellationToken);
                }
            }

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}
