using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Metadata;

public class TOriginConfiguration : IEntityTypeConfiguration<TOriginEntity>
{
    public void Configure(EntityTypeBuilder<TOriginEntity> builder) 
    {
        builder.ToTable("t_origin", "metadata");
        builder.HasKey(x => new { x.OriginId });
            
        builder.Property(x => x.OriginId).HasColumnName("origin_id").IsRequired();
        builder.Property(x => x.OriginWhen).HasColumnName("origin_when").IsRequired();
        builder.Property(x => x.OriginDescription).HasColumnName("origin_description").IsUnicode(false).HasMaxLength(1000);
        builder.Property(x => x.OriginReason).HasColumnName("origin_reason").IsUnicode(false).HasMaxLength(1000);
        builder.Property(x => x.OriginSource).HasColumnName("origin_source").IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.OrganizationId).HasColumnName("organization_id").IsRequired();
        builder.Property(x => x.ProxyAgent).HasColumnName("proxy_agent");
        builder.Property(x => x.ProxySubject).HasColumnName("proxy_subject");

    }
}