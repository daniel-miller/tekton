namespace Tek.Service.Timeline;

public partial class TAggregateEntity
{
    public Guid AggregateId { get; set; }
    public Guid AggregateRoot { get; set; }

    public string AggregateType { get; set; } = null!;
}