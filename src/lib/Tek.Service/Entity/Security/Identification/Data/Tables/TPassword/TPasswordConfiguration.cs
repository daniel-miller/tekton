using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Security;

public class TPasswordConfiguration : IEntityTypeConfiguration<TPasswordEntity>
{
    public void Configure(EntityTypeBuilder<TPasswordEntity> builder) 
    {
        builder.ToTable("t_password", "security");
        builder.HasKey(x => new { x.PasswordId });
            
        builder.Property(x => x.PasswordId).HasColumnName("password_id").IsRequired();
        builder.Property(x => x.EmailId).HasColumnName("email_id").IsRequired();
        builder.Property(x => x.EmailAddress).HasColumnName("email_address").IsUnicode(false).HasMaxLength(254);
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.PasswordExpiry).HasColumnName("password_expiry").IsRequired();
        builder.Property(x => x.DefaultPlaintext).HasColumnName("default_plaintext").IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.DefaultExpiry).HasColumnName("default_expiry");
        builder.Property(x => x.CreatedWhen).HasColumnName("created_when").IsRequired();
        builder.Property(x => x.LastForgottenWhen).HasColumnName("last_forgotten_when");
        builder.Property(x => x.LastModifiedWhen).HasColumnName("last_modified_when");

    }
}