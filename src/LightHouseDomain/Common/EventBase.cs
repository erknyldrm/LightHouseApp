namespace LightHouseDomain.Common;

public abstract class EventBase : IEvent
{
    public Guid EventId { get; protected set; }
    public DateTime OccurredAt { get; protected set; }
    public string EventType { get; protected set; }
    public Guid AggregateId { get; protected set; }

    protected EventBase()
    {
        EventId = Guid.NewGuid();
        OccurredAt = DateTime.UtcNow;
        EventType = GetType().Name;
        AggregateId = Guid.Empty;
    }

    protected EventBase(Guid aggregateId) : this()
    {
        AggregateId = aggregateId;
    }
}
