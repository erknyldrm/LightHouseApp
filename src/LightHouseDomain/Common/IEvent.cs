namespace LightHouseDomain.Common;

public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
    string EventType { get; }
    Guid AggregateId { get; }
}
