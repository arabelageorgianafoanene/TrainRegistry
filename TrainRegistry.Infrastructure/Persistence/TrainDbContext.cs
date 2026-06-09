using Microsoft.EntityFrameworkCore;
using TrainRegistry.Domain;

namespace TrainRegistry.Infrastructure.Persistence
{
    public class TrainDbContext : DbContext
    {
        public TrainDbContext(DbContextOptions dbContextOptions): base(dbContextOptions) { }
        public DbSet<Train> Trains => Set<Train>();

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
