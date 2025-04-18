using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tek.Service.Contact;

public class TProvinceConfiguration : IEntityTypeConfiguration<TProvinceEntity>
{
    public void Configure(EntityTypeBuilder<TProvinceEntity> builder) 
    {
        builder.ToTable("t_province", "location");
        builder.HasKey(x => new { x.ProvinceId });
            
        builder.Property(x => x.ProvinceId).HasColumnName("province_id").IsRequired();
        builder.Property(x => x.ProvinceCode).HasColumnName("province_code").IsUnicode(false).HasMaxLength(2);
        builder.Property(x => x.ProvinceName).HasColumnName("province_name").IsRequired().IsUnicode(false).HasMaxLength(80);
        builder.Property(x => x.CountryCode).HasColumnName("country_code").IsRequired().IsUnicode(false).HasMaxLength(2);
        builder.Property(x => x.CountryId).HasColumnName("country_id");

    }
}