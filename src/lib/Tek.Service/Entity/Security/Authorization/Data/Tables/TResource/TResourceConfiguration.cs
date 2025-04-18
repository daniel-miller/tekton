using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Security;

public class TResourceConfiguration : IEntityTypeConfiguration<TResourceEntity>
{
    public void Configure(EntityTypeBuilder<TResourceEntity> builder) 
    {
        builder.ToTable("t_resource", "security");
        builder.HasKey(x => new { x.ResourceId });
            
        builder.Property(x => x.ResourceId).HasColumnName("resource_id").IsRequired();
        builder.Property(x => x.ResourceType).HasColumnName("resource_type").IsRequired().IsUnicode(false).HasMaxLength(30);
        builder.Property(x => x.ResourceName).HasColumnName("resource_name").IsRequired().IsUnicode(false).HasMaxLength(100);

    }
}