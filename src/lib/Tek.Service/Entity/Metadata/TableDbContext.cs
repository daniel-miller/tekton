using Microsoft.EntityFrameworkCore;

using Tek.Service.Bus;
using Tek.Service.Contact;
using Tek.Service.Content;
using Tek.Service.Metadata;
using Tek.Service.Security;

namespace Tek.Service;

public class TableDbContext : DbContext
{
    public TableDbContext(DbContextOptions options) : base(options) { }

    #region Storage Tables

    // Application: Contact
    internal DbSet<TCountryEntity> TCountry { get; set; }
    internal DbSet<TProvinceEntity> TProvince { get; set; }

    // Application: Content
    internal DbSet<TTranslationEntity> TTranslation { get; set; }

    // Utility: Bus
    internal DbSet<TAggregateEntity> TAggregate { get; set; }
    internal DbSet<TEventEntity> TEvent { get; set; }

    // Utility: Metadata
    internal DbSet<TEntityEntity> TEntity { get; set; }
    internal DbSet<TOriginEntity> TOrigin { get; set; }
    internal DbSet<TVersionEntity> TVersion { get; set; }

    // Utility: Security
    internal DbSet<TOrganizationEntity> TOrganization { get; set; }
    internal DbSet<TPartitionEntity> TPartition { get; set; }
    internal DbSet<TPasswordEntity> TPassword { get; set; }
    internal DbSet<TPermissionEntity> TPermission { get; set; }
    internal DbSet<TResourceEntity> TResource { get; set; }
    internal DbSet<TRoleEntity> TRole { get; set; }
    internal DbSet<TSecretEntity> TSecret { get; set; }



    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ApplyConfigurations(builder);
        ApplyNavigations(builder);

        var decimalProperties = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

        foreach (var property in decimalProperties)
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
    }

    private void ApplyConfigurations(ModelBuilder builder)
    {
        // Application: Contact
        builder.ApplyConfiguration(new TCountryConfiguration());
        builder.ApplyConfiguration(new TProvinceConfiguration());

        // Application: Content
        builder.ApplyConfiguration(new TTranslationConfiguration());

        // Utility: Bus
        builder.ApplyConfiguration(new TAggregateConfiguration());
        builder.ApplyConfiguration(new TEventConfiguration());

        // Utility: Metadata
        builder.ApplyConfiguration(new TEntityConfiguration());
        builder.ApplyConfiguration(new TOriginConfiguration());
        builder.ApplyConfiguration(new TVersionConfiguration());

        // Utility: Security
        builder.ApplyConfiguration(new TOrganizationConfiguration());
        builder.ApplyConfiguration(new TPartitionConfiguration());
        builder.ApplyConfiguration(new TPasswordConfiguration());
        builder.ApplyConfiguration(new TPermissionConfiguration());
        builder.ApplyConfiguration(new TResourceConfiguration());
        builder.ApplyConfiguration(new TRoleConfiguration());
        builder.ApplyConfiguration(new TSecretConfiguration());


    }

    private void ApplyNavigations(ModelBuilder builder)
    {
        // builder.Entity<PrimaryEntity>()
        //     .HasMany(e => e.ForeignProperty)
        //     .WithOne(e => e.PrimaryProperty)
        //     .HasForeignKey(e => e.ForeignIdentifier)
        //     .HasPrincipalKey(e => e.PrimaryIdentifier);
    }
}