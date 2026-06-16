using Microsoft.EntityFrameworkCore;
using TrainRegistry.Domain.Entities;

namespace TrainRegistry.Infrastructure.Persistence
{
    public class TrainDbContext : DbContext
    {
        public TrainDbContext(DbContextOptions dbContextOptions): base(dbContextOptions) { }
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
    }
}
