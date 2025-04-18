using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Bus;

public class TAggregateConfiguration : IEntityTypeConfiguration<TAggregateEntity>
{
    public void Configure(EntityTypeBuilder<TAggregateEntity> builder) 
    {
        builder.ToTable("t_aggregate", "bus");
        builder.HasKey(x => new { x.AggregateId });
            
        builder.Property(x => x.AggregateId).HasColumnName("aggregate_id").IsRequired();
        builder.Property(x => x.AggregateType).HasColumnName("aggregate_type").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.AggregateRoot).HasColumnName("aggregate_root").IsRequired();

    }
}