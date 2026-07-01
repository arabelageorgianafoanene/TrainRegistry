using Microsoft.EntityFrameworkCore;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Infrastructure.Persistence
{
    public class TrainDbContext : DbContext
    {
        public TrainDbContext(DbContextOptions dbContextOptions): base(dbContextOptions){ }
        public DbSet<Train> Trains => Set<Train>();

        public DbSet<User> Users => Set<User>();

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
    }
}
