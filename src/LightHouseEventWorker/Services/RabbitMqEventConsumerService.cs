using System.Text.Json;
using LightHouseDomain.Events.Photo;
using LightHouseEventWorker.EventHandlers;
using LightHouseInfrastructure.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LightHouseEventWorker.Services;

public class RabbitMqEventConsumerService : BackgroundService
{
    private class EventMessage
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
        public Guid AggregateId { get; set; }
        public JsonElement Data { get; set; }
    }

    private readonly ILogger<RabbitMqEventConsumerService> _logger;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly JsonSerializerOptions _jsonSerializerOptions;
    public RabbitMqEventConsumerService(
        ILogger<RabbitMqEventConsumerService> logger,
        IOptions<RabbitMqSettings> rabbitMqSettings,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _rabbitMqSettings = rabbitMqSettings.Value;
        _serviceProvider = serviceProvider;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    private async Task InitializeRabbitMqAsync(CancellationToken cancellationToken)
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.HostName,
                UserName = _rabbitMqSettings.UserName,
                Password = _rabbitMqSettings.Password,
                Port = _rabbitMqSettings.Port,
                VirtualHost = _rabbitMqSettings.VirtualHost
            };

            _connection = await factory.CreateConnectionAsync();

            _logger.LogInformation("RabbitMQ connection established. Host: {HostName} Port:{Port}", _rabbitMqSettings.HostName, _rabbitMqSettings.Port);

            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("RabbitMQ channel created.");

            await _channel.ExchangeDeclareAsync(
                exchange: _rabbitMqSettings.ExchangeName,
                type: ExchangeType.Topic,
                durable: _rabbitMqSettings.Durable,
                autoDelete: _rabbitMqSettings.AutoDelete,
                arguments: null,
                cancellationToken: cancellationToken);

            _logger.LogInformation("RabbitMQ exchange declared. Exchange: {ExchangeName}", _rabbitMqSettings.ExchangeName);

            await _channel.QueueDeclareAsync(
                queue: _rabbitMqSettings.QueueName,
                durable: _rabbitMqSettings.Durable,
                exclusive: false,
                autoDelete: _rabbitMqSettings.AutoDelete,
                arguments: null,
                cancellationToken: cancellationToken);

            _logger.LogInformation("RabbitMQ queue declared. Queue: {QueueName}", _rabbitMqSettings.QueueName);

            await _channel.QueueBindAsync(
                queue: _rabbitMqSettings.QueueName,
                exchange: _rabbitMqSettings.ExchangeName,
                routingKey: "ligght.house.photo.*",
                arguments: null,
                cancellationToken: cancellationToken);

            _logger.LogInformation("RabbitMQ queue bound to exchange. Queue: {QueueName} Exchange: {ExchangeName}", _rabbitMqSettings.QueueName, _rabbitMqSettings.ExchangeName);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error initializing RabbitMQ");
        }
    }

    private async Task HandlePhotoUploadedEventAsync(EventMessage eventMessage, CancellationToken cancellationToken)
    {
        try
        {
            var dataElement = eventMessage.Data;
            var fileName = dataElement.GetProperty("fileName").GetString() ?? string.Empty;
            var photoId = dataElement.GetProperty("photoId").GetGuid();
            var lightHouseId = dataElement.GetProperty("lightHouseId").GetGuid();
            var userId = dataElement.GetProperty("userId").GetGuid();
            var cameraType = dataElement.GetProperty("cameraType").GetString() ?? string.Empty;
            var resolution = dataElement.GetProperty("resolution").GetString() ?? string.Empty;
            var lens = dataElement.GetProperty("lens").GetString() ?? string.Empty;
            var uploadedAt = dataElement.GetProperty("uploadedAt").GetDateTime();

            var photoUploadedEvent = new PhotoUploaded(
                photoId, fileName, userId, lightHouseId, cameraType, resolution, lens, uploadedAt
            );

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IPhotoUploadedEventHandler>();

            await handler.HandleAsync(photoUploadedEvent, cancellationToken);

            _logger.LogInformation("Processed PhotoUploadedEvent - EventId: {EventId}", eventMessage.EventId);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error handling PhotoUploadedEvent - EventId: {EventId}", eventMessage.EventId);
        }
    }

    private async Task ProcessEventAsync(string message, string routingKey, CancellationToken cancellationToken)
    {
        try
        {
            var eventMessage = JsonSerializer.Deserialize<EventMessage>(message);
            if (eventMessage == null)
            {
                _logger.LogWarning("Failed to deserialize EventMessage from message: {Message}", message);
                return;
            }

            switch (eventMessage.EventType)
            {
                case "PhotoUploaded":
                    await HandlePhotoUploadedEventAsync(eventMessage, cancellationToken);
                    break;
                default:
                    _logger.LogWarning("Unhandled event type: {EventType}", eventMessage.EventType);
                    break;
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error processing event message: {Message} - RoutingKey:{RoutingKey}", message, routingKey);
        }

        throw new NotImplementedException();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _logger.LogInformation("RabbitMqEventConsumerService is starting.");

        await InitializeRabbitMqAsync(stoppingToken);

        if (_channel == null)
        {
            _logger.LogError("RabbitMQ channel is not initialized.");
            return;
        }

        _logger.LogInformation("RabbitMqEventConsumerService is running.");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;

                _logger.LogInformation("Received message with RoutingKey: {RoutingKey}", routingKey);

                await ProcessEventAsync(message, routingKey, stoppingToken);

                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

                await _channel.BasicConsumeAsync(
                    queue: _rabbitMqSettings.QueueName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: stoppingToken);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error handling received message.");
            }
        };
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMqEventConsumerService is stopping.");

        if (_channel != null)
        {
            await _channel.CloseAsync(cancellationToken: cancellationToken);

            await _channel.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);

        _logger.LogInformation("RabbitMqEventConsumerService has stopped.");
    }
}
