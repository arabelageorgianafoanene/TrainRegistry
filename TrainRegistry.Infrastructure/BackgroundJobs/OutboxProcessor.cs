using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TrainEventContracts.Common;
using TrainRegistry.Application.Abstractions;
using TrainRegistry.Infrastructure.Mappers;
using TrainRegistry.Infrastructure.Persistence;

namespace TrainRegistry.Infrastructure.BackgroundJobs
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessor> _logger;

        public OutboxProcessor(IServiceScopeFactory serviceScopeFactory, ILogger<OutboxProcessor> logger)
        {
            _scopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<TrainDbContext>();
                    var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

                    var messages = await db.OutboxMessages
                        .Where(x => !x.Processed)
                        .OrderBy(x => x.UpdatedOnUtc)
                        .Take(100)
                        .ToListAsync(cancellationToken);

                    foreach (var msg in messages)
                    {
                        var integrationEvent = JsonSerializer.Deserialize<TrainEvent>(msg.Payload);

                        if (integrationEvent is null) continue;

                        var routingKey = RoutingKeyMapper.Map(integrationEvent.GetType().Name);
                        await publisher.Publish(integrationEvent, routingKey, cancellationToken);

                        msg.Processed = true;
                        msg.UpdatedOnUtc = DateTime.UtcNow;
                    }

                    await db.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing outbox messages");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}

