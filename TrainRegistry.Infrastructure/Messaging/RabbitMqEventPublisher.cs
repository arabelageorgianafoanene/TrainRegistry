using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TrainRegistry.Application.Abstractions;
using TrainEventContracts;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Logging;

namespace TrainRegistry.Infrastructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly RabbitMqConnection _rabbitMqConnection;
        private readonly ILogger<RabbitMqEventPublisher> _logger;


        public RabbitMqEventPublisher(RabbitMqConnection rabbitMqConnection, ILogger<RabbitMqEventPublisher> logger)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _logger = logger;
        }

        public async Task Publish<T>(T contractEvent, string routingKey, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(contractEvent);

            ArgumentException.ThrowIfNullOrEmpty(routingKey);

            var channelOptions = new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: true);

            await using var channel = await _rabbitMqConnection.Connection.CreateChannelAsync(channelOptions, cancellationToken: ct);

            await channel.ExchangeDeclareAsync(
                exchange: TrainEventRoutingKeys.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                cancellationToken: ct);

            var json = JsonSerializer.Serialize(contractEvent, contractEvent.GetType());
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            channel.BasicReturnAsync += (sender, e) =>
            {
                _logger.LogError(
                    "Message returned — could not route to {Exchange}/{RoutingKey}: {ReplyCode} {ReplyText}",
                    e.Exchange, e.RoutingKey, e.ReplyCode, e.ReplyText);
                return Task.CompletedTask;
            };


            try
            {

             await channel.BasicPublishAsync(
                exchange: TrainEventRoutingKeys.ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body,
                cancellationToken: ct);
            }
            catch(BrokerUnreachableException exception)
            {
                _logger.LogError("Broker is unreachable!");
                throw;
            }
            catch (PublishException publishException) 
            {
                if(!publishException.IsReturn)
                {
                    _logger.LogError("The broker responded with nack to my message!");
                }

                throw;
            }
        }
    }
}
