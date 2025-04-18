using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TResourceReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TResourceAdapter _adapter;

    public TResourceReader(IDbContextFactory<TableDbContext> context, TResourceAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid resource, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TResource
            .AnyAsync(x => x.ResourceId == resource, token);
    }

    public async Task<TResourceEntity?> FetchAsync(Guid resource, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TResource
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ResourceId == resource, token);
    }

    public async Task<int> CountAsync(IResourceCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TResourceEntity>> CollectAsync(IResourceCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<ResourceMatch>> SearchAsync(IResourceCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TResourceEntity> BuildQuery(TableDbContext db, IResourceCriteria criteria)
    {
        var query = db.TResource.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.ResourceId != null)
        //    query = query.Where(x => x.ResourceId == criteria.ResourceId);

        // if (criteria.ResourceType != null)
        //    query = query.Where(x => x.ResourceType == criteria.ResourceType);

        // if (criteria.ResourceName != null)
        //    query = query.Where(x => x.ResourceName == criteria.ResourceName);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}