using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRegistry.Domain.Abstractions;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Infrastructure.Persistence
{
    public class TrainDbContext : DbContext
    {
        private readonly IPublisher _publisher;

        public TrainDbContext(DbContextOptions dbContextOptions, IPublisher publisher): base(dbContextOptions) 
        {
            _publisher = publisher;

        }
        public DbSet<Train> Trains => Set<Train>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Train>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired();
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Select(e => e.Entity)
                .SelectMany(e =>
                {
                    var events = e.DomainEvents.ToList();
                    e.ClearDomainEvents();
                    return events;
                });
                
                                    
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
            
            return result;
        }
    }
}
