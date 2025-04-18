using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Security;

public class TSecretConfiguration : IEntityTypeConfiguration<TSecretEntity>
{
    public void Configure(EntityTypeBuilder<TSecretEntity> builder) 
    {
        builder.ToTable("t_secret", "security");
        builder.HasKey(x => new { x.SecretId });
            
        builder.Property(x => x.PasswordId).HasColumnName("password_id").IsRequired();
        builder.Property(x => x.SecretId).HasColumnName("secret_id").IsRequired();
        builder.Property(x => x.SecretType).HasColumnName("secret_type").IsRequired().IsUnicode(false).HasMaxLength(30);
        builder.Property(x => x.SecretName).HasColumnName("secret_name").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.SecretValue).HasColumnName("secret_value").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.SecretScope).HasColumnName("secret_scope").IsRequired().IsUnicode(false).HasMaxLength(30);
        builder.Property(x => x.SecretExpiry).HasColumnName("secret_expiry").IsRequired();
        builder.Property(x => x.SecretLimetimeLimit).HasColumnName("secret_limetime_limit");

    }
}