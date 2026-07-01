using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TrainRegistry.Infrastructure.Persistence
{
    public class TrainDbContextFactory : IDesignTimeDbContextFactory<TrainDbContext>
    {
        TrainDbContext IDesignTimeDbContextFactory<TrainDbContext>.CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TrainDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new TrainDbContext(optionsBuilder.Options);
        }
    }
}
