using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Metadata;

public class TEntityConfiguration : IEntityTypeConfiguration<TEntityEntity>
{
    public void Configure(EntityTypeBuilder<TEntityEntity> builder) 
    {
        builder.ToTable("t_entity", "metadata");
        builder.HasKey(x => new { x.EntityId });
            
        builder.Property(x => x.StorageStructure).HasColumnName("storage_structure").IsRequired().IsUnicode(false).HasMaxLength(20);
        builder.Property(x => x.StorageSchema).HasColumnName("storage_schema").IsRequired().IsUnicode(false).HasMaxLength(30);
        builder.Property(x => x.StorageTable).HasColumnName("storage_table").IsRequired().IsUnicode(false).HasMaxLength(40);
        builder.Property(x => x.StorageTableFuture).HasColumnName("storage_table_future").IsUnicode(false).HasMaxLength(40);
        builder.Property(x => x.StorageKey).HasColumnName("storage_key").IsRequired().IsUnicode(false).HasMaxLength(80);
        builder.Property(x => x.ComponentType).HasColumnName("component_type").IsRequired().IsUnicode(false).HasMaxLength(20);
        builder.Property(x => x.ComponentName).HasColumnName("component_name").IsRequired().IsUnicode(false).HasMaxLength(30);
        builder.Property(x => x.ComponentFeature).HasColumnName("component_feature").IsRequired().IsUnicode(false).HasMaxLength(40);
        builder.Property(x => x.EntityName).HasColumnName("entity_name").IsRequired().IsUnicode(false).HasMaxLength(50);
        builder.Property(x => x.EntityId).HasColumnName("entity_id").IsRequired();
        builder.Property(x => x.CollectionSlug).HasColumnName("collection_slug").IsRequired().IsUnicode(false).HasMaxLength(50);
        builder.Property(x => x.CollectionKey).HasColumnName("collection_key").IsRequired().IsUnicode(false).HasMaxLength(60);

    }
}