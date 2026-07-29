using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using TrainRegistry.Domain.Abstractions;
using TrainRegistry.Infrastructure.Mappers;
using TrainRegistry.Infrastructure.Outbox;

namespace TrainRegistry.Infrastructure.Persistence
{
    public class OutboxSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is null)
            {
                throw new InvalidOperationException("Saving changes fired without a valid dbcontext!");
            }

            InsertOutboxMessages(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
                
        private static void InsertOutboxMessages( DbContext dbContext)
        {
           var entities = dbContext.ChangeTracker.Entries<Entity>().
                Where(e=>e.Entity.DomainEvents.Any()).ToList();

            if (!entities.Any()) return;

            foreach (var entity in entities)
            {
                foreach(var domainEvent in entity.Entity.DomainEvents)
                {
                    var integrationEvent = IntegrationEventMapper.Map(domainEvent);

                    var outbox = new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        Payload = JsonSerializer.Serialize(integrationEvent),
                        UpdatedOnUtc = DateTime.UtcNow,
                        Processed = false,
                        Type = integrationEvent.GetType().Name
                    };

                    dbContext.Add(outbox);
                }

                entity.Entity.ClearDomainEvents();
            }
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
        {
            if (eventData.Context is null)
            {
                throw new InvalidOperationException("Saving changes fired without a valid dbcontext!");
            }

            InsertOutboxMessages(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }

}
