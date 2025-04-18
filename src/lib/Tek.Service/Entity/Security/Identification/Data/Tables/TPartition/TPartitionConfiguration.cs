using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Security;

public class TPartitionConfiguration : IEntityTypeConfiguration<TPartitionEntity>
{
    public void Configure(EntityTypeBuilder<TPartitionEntity> builder) 
    {
        builder.ToTable("t_partition", "security");
        builder.HasKey(x => new { x.PartitionNumber });
            
        builder.Property(x => x.PartitionNumber).HasColumnName("partition_number").IsRequired();
        builder.Property(x => x.PartitionSlug).HasColumnName("partition_slug").IsRequired().IsUnicode(false).HasMaxLength(3);
        builder.Property(x => x.PartitionName).HasColumnName("partition_name").IsRequired().IsUnicode(false).HasMaxLength(50);
        builder.Property(x => x.PartitionHost).HasColumnName("partition_host").IsRequired().IsUnicode(false).HasMaxLength(50);
        builder.Property(x => x.PartitionEmail).HasColumnName("partition_email").IsRequired().IsUnicode(false).HasMaxLength(254);
        builder.Property(x => x.PartitionSettings).HasColumnName("partition_settings").IsUnicode(false);
        builder.Property(x => x.PartitionTesters).HasColumnName("partition_testers").IsUnicode(false).HasMaxLength(400);
        builder.Property(x => x.ModifiedWhen).HasColumnName("modified_when").IsRequired();

    }
}