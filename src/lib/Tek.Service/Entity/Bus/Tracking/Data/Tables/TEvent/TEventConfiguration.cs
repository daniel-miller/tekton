using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Bus;

public class TEventConfiguration : IEntityTypeConfiguration<TEventEntity>
{
    public void Configure(EntityTypeBuilder<TEventEntity> builder) 
    {
        builder.ToTable("t_event", "bus");
        builder.HasKey(x => new { x.EventId });
            
        builder.Property(x => x.EventId).HasColumnName("event_id").IsRequired();
        builder.Property(x => x.EventType).HasColumnName("event_type").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.EventData).HasColumnName("event_data").IsRequired().IsUnicode(false);
        builder.Property(x => x.AggregateId).HasColumnName("aggregate_id").IsRequired();
        builder.Property(x => x.AggregateVersion).HasColumnName("aggregate_version").IsRequired();
        builder.Property(x => x.OriginId).HasColumnName("origin_id").IsRequired();

    }
}