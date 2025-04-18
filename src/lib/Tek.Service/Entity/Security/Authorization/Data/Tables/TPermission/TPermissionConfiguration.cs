using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Security;

public class TPermissionConfiguration : IEntityTypeConfiguration<TPermissionEntity>
{
    public void Configure(EntityTypeBuilder<TPermissionEntity> builder) 
    {
        builder.ToTable("t_permission", "security");
        builder.HasKey(x => new { x.PermissionId });
            
        builder.Property(x => x.PermissionId).HasColumnName("permission_id").IsRequired();
        builder.Property(x => x.AccessType).HasColumnName("access_type").IsRequired().IsUnicode(false).HasMaxLength(10);
        builder.Property(x => x.AccessFlags).HasColumnName("access_flags").IsRequired();
        builder.Property(x => x.ResourceId).HasColumnName("resource_id").IsRequired();
        builder.Property(x => x.RoleId).HasColumnName("role_id").IsRequired();

    }
}