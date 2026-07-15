using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TrainRegistry.Application.Abstractions;

namespace TrainRegistry.Infrastructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private const string ExchangeName = "train_events_exchange";
        private readonly RabbitMqConnection _rabbitMqConnection;


        public RabbitMqEventPublisher(RabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task Publish<T>(T contractEvent, string routingKey, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(contractEvent);

            ArgumentException.ThrowIfNullOrEmpty(routingKey);

            await using var channel = await _rabbitMqConnection.Connection.CreateChannelAsync(cancellationToken: ct);

            await channel.ExchangeDeclareAsync(
                exchange: ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                cancellationToken: ct);

            var json = JsonSerializer.Serialize(contractEvent, contractEvent.GetType());
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: ct);
        }
    }
}
