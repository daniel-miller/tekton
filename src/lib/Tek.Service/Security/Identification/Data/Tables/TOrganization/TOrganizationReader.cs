using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TOrganizationReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TOrganizationAdapter _adapter;

    public TOrganizationReader(IDbContextFactory<TableDbContext> context, TOrganizationAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid organization, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TOrganization
            .AnyAsync(x => x.OrganizationId == organization, token);
    }

    public async Task<TOrganizationEntity?> FetchAsync(Guid organization, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TOrganization
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OrganizationId == organization, token);
    }

    public async Task<int> CountAsync(IOrganizationCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TOrganizationEntity>> CollectAsync(IOrganizationCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<OrganizationMatch>> SearchAsync(IOrganizationCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TOrganizationEntity> BuildQuery(TableDbContext db, IOrganizationCriteria criteria)
    {
        var query = db.TOrganization.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.OrganizationId != null)
        //    query = query.Where(x => x.OrganizationId == criteria.OrganizationId);

        // if (criteria.OrganizationNumber != null)
        //    query = query.Where(x => x.OrganizationNumber == criteria.OrganizationNumber);

        // if (criteria.OrganizationSlug != null)
        //    query = query.Where(x => x.OrganizationSlug == criteria.OrganizationSlug);

        // if (criteria.OrganizationName != null)
        //    query = query.Where(x => x.OrganizationName == criteria.OrganizationName);

        // if (criteria.PartitionNumber != null)
        //    query = query.Where(x => x.PartitionNumber == criteria.PartitionNumber);

        // if (criteria.ModifiedWhen != null)
        //    query = query.Where(x => x.ModifiedWhen == criteria.ModifiedWhen);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}