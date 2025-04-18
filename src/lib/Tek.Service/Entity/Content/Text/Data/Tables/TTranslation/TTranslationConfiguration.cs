using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Content;

public class TTranslationConfiguration : IEntityTypeConfiguration<TTranslationEntity>
{
    public void Configure(EntityTypeBuilder<TTranslationEntity> builder) 
    {
        builder.ToTable("t_translation", "content");
        builder.HasKey(x => new { x.TranslationId });
            
        builder.Property(x => x.TranslationId).HasColumnName("translation_id").IsRequired();
        builder.Property(x => x.TranslationText).HasColumnName("translation_text").IsRequired().IsUnicode(false);
        builder.Property(x => x.ModifiedWhen).HasColumnName("modified_when").IsRequired();

    }
}