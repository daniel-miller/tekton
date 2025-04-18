namespace Tek.Service.Bus;

public partial class TEventEntity
{
    public Guid AggregateId { get; set; }
    public Guid EventId { get; set; }
    public Guid OriginId { get; set; }

    public string EventData { get; set; } = null!;
    public string EventType { get; set; } = null!;

    public int AggregateVersion { get; set; }
}