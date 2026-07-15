

namespace TrainRegistry.Application.Abstractions
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event, string routingKey, CancellationToken ct = default);
    }
}
