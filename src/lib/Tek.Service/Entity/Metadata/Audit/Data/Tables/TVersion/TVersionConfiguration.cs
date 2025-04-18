using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Metadata;

public class TVersionConfiguration : IEntityTypeConfiguration<TVersionEntity>
{
    public void Configure(EntityTypeBuilder<TVersionEntity> builder) 
    {
        builder.ToTable("t_version", "metadata");
        builder.HasKey(x => new { x.VersionNumber });
            
        builder.Property(x => x.VersionNumber).HasColumnName("version_number").IsRequired();
        builder.Property(x => x.VersionType).HasColumnName("version_type").IsRequired().IsUnicode(false).HasMaxLength(20);
        builder.Property(x => x.VersionName).HasColumnName("version_name").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.ScriptPath).HasColumnName("script_path").IsRequired().IsUnicode(false).HasMaxLength(300);
        builder.Property(x => x.ScriptContent).HasColumnName("script_content").IsRequired().IsUnicode(false);
        builder.Property(x => x.ScriptExecuted).HasColumnName("script_executed").IsRequired();

    }
}