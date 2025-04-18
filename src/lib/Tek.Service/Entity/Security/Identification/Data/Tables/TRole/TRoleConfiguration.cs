using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Security;

public class TRoleConfiguration : IEntityTypeConfiguration<TRoleEntity>
{
    public void Configure(EntityTypeBuilder<TRoleEntity> builder) 
    {
        builder.ToTable("t_role", "security");
        builder.HasKey(x => new { x.RoleId });
            
        builder.Property(x => x.RoleId).HasColumnName("role_id").IsRequired();
        builder.Property(x => x.RoleType).HasColumnName("role_type").IsRequired().IsUnicode(false).HasMaxLength(20);
        builder.Property(x => x.RoleName).HasColumnName("role_name").IsRequired().IsUnicode(false).HasMaxLength(100);

    }
}