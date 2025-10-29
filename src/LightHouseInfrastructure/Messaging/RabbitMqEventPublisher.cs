using System.Text.Json;
using LightHouseDomain.Common;
using LightHouseInfrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LightHouseInfrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IChannel _channel;
    private bool _exchangeDeclared;
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private bool _disposed;



    public RabbitMqEventPublisher(IOptions<RabbitMqSettings> rabbitMqSettings, ILogger<RabbitMqEventPublisher> logger)
    {
        _rabbitMqSettings = rabbitMqSettings.Value ?? throw new ArgumentNullException(nameof(rabbitMqSettings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.HostName,
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password,
            VirtualHost = _rabbitMqSettings.VirtualHost
        };

        _logger.LogInformation("RabbitMQ Event Publisher initialized with Host: {Host}, Port: {Port}, VirtualHost: {VirtualHost}",
            _rabbitMqSettings.HostName, _rabbitMqSettings.Port, _rabbitMqSettings.VirtualHost);
    }

    private async Task EnsureConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (_connection != null && _channel != null && _connection.IsOpen && _exchangeDeclared)
            return;

        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            if (_connection == null)
            {
                _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
                _logger.LogInformation("RabbitMQ connection created.");
            }

            if (_channel == null)
            {
                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
                _logger.LogInformation("RabbitMQ channel created.");
            }

            if (!_exchangeDeclared)
            {
                await _channel.ExchangeDeclareAsync(
                    exchange: _rabbitMqSettings.ExchangeName,
                    type: ExchangeType.Fanout,
                    durable: true,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                _exchangeDeclared = true;
                _logger.LogInformation("RabbitMQ exchange '{ExchangeName}' declared.", _rabbitMqSettings.ExchangeName);
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        _logger.LogInformation("RabbitMQ connection established.");
    }


    public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@event);

        await EnsureConnectionAsync(cancellationToken);

        try
        {
            var eventMessage = new EventMessage
            {
                EventId = @event.EventId,
                EventType = @event.GetType().FullName ?? string.Empty,
                OccuredAt = @event.OccurredAt,
                AggregateId = @event.AggregateId,
                Data = @event
            };

            var json = System.Text.Json.JsonSerializer.Serialize(eventMessage, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var body = System.Text.Encoding.UTF8.GetBytes(json);

            var routingKey = $"lighthouse.{@event.EventType.ToLowerInvariant()}";

            var basicProperties = new BasicProperties
            {
                ContentType = "application/json",
                Persistent = true,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel.BasicPublishAsync(
                exchange: _rabbitMqSettings.ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: basicProperties,
                body: body,
                cancellationToken: cancellationToken);

            _logger.LogInformation("Published event {EventType} with ID {EventId} to exchange {Exchange} with routing key {RoutingKey}",
                @event.GetType().FullName, @event.EventId, _rabbitMqSettings.ExchangeName, routingKey);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType} with ID {EventId}", @event.GetType().FullName, @event.EventId);
            throw;
        }
    }
    public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);

        foreach (var @event in events)
        {
            await PublishAsync(@event, cancellationToken);
        }   
    }

    public async ValueTask DisposeAsync()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        _semaphoreSlim.Dispose();
        _disposed = true;

        _logger.LogInformation("RabbitMQ Event Publisher disposed.");

        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();    
    }


    private record EventMessage
    {
        public Guid EventId { get; set; }
        public string EventType { get; init; } = string.Empty;
        public DateTime OccuredAt { get; init; }
        public Guid AggregateId { get; set; }
        public object Data { get; set; } = null;
    }
}
