using Microsoft.EntityFrameworkCore;
using System;
using TrainRegistry.Domain.Entities;
using TrainRegistry.Infrastructure.Outbox;

namespace TrainRegistry.Infrastructure.Persistence
{
    public class TrainDbContext : DbContext
    {
        private readonly OutboxSaveChangesInterceptor _outboxInterceptor;

        public TrainDbContext(DbContextOptions dbContextOptions, OutboxSaveChangesInterceptor outboxSaveChangesInterceptor) : base(dbContextOptions)
        {
            _outboxInterceptor = outboxSaveChangesInterceptor;
        }
        public DbSet<Train> Trains => Set<Train>();

        public DbSet<User> Users => Set<User>();

        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Train>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Name).IsRequired();

                builder.OwnsOne(t => t.Length, length =>
                {
                    length.Property(l => l.Value).HasColumnName("Length").IsRequired();
                });

                builder.OwnsOne(t => t.Speed, speed =>
                {
                    speed.Property(s => s.Value).HasColumnName("Speed").IsRequired();
                });

                builder.OwnsOne(t => t.TrainStatus, status =>
                {
                    status.Property(s => s.Value).HasColumnName("TrainStatus").IsRequired();
                });
            });
        }
              
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_outboxInterceptor);
        }
    }
}
