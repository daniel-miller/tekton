using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Security;

public class TOrganizationConfiguration : IEntityTypeConfiguration<TOrganizationEntity>
{
    public void Configure(EntityTypeBuilder<TOrganizationEntity> builder) 
    {
        builder.ToTable("t_organization", "security");
        builder.HasKey(x => new { x.OrganizationId });
            
        builder.Property(x => x.OrganizationId).HasColumnName("organization_id").IsRequired();
        builder.Property(x => x.OrganizationNumber).HasColumnName("organization_number").IsRequired();
        builder.Property(x => x.OrganizationSlug).HasColumnName("organization_slug").IsRequired().IsUnicode(false).HasMaxLength(3);
        builder.Property(x => x.OrganizationName).HasColumnName("organization_name").IsRequired().IsUnicode(false).HasMaxLength(50);
        builder.Property(x => x.PartitionNumber).HasColumnName("partition_number").IsRequired();
        builder.Property(x => x.ModifiedWhen).HasColumnName("modified_when").IsRequired();

    }
}