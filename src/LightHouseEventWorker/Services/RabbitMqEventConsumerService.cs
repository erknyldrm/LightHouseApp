using LightHouseInfrastructure.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LightHouseEventWorker.Services;

public class RabbitMqEventConsumerService : BackgroundService
{
    private class EventMessage
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
        public Guid AggregateId { get; set; }
        public Object Payload { get; set; } = new();
    }

    private readonly ILogger<RabbitMqEventConsumerService> _logger;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqEventConsumerService(
        ILogger<RabbitMqEventConsumerService> logger,
        IOptions<RabbitMqSettings> rabbitMqSettings,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _rabbitMqSettings = rabbitMqSettings.Value;
        _serviceProvider = serviceProvider;
    }

    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
