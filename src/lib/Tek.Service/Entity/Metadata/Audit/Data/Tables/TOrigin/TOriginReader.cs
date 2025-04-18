using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Metadata;

public class TOriginReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TOriginAdapter _adapter;

    public TOriginReader(IDbContextFactory<TableDbContext> context, TOriginAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid origin, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TOrigin
            .AnyAsync(x => x.OriginId == origin, token);
    }

    public async Task<TOriginEntity?> FetchAsync(Guid origin, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TOrigin
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OriginId == origin, token);
    }

    public async Task<int> CountAsync(IOriginCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TOriginEntity>> CollectAsync(IOriginCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<OriginMatch>> SearchAsync(IOriginCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TOriginEntity> BuildQuery(TableDbContext db, IOriginCriteria criteria)
    {
        var query = db.TOrigin.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.OriginId != null)
        //    query = query.Where(x => x.OriginId == criteria.OriginId);

        // if (criteria.OriginWhen != null)
        //    query = query.Where(x => x.OriginWhen == criteria.OriginWhen);

        // if (criteria.OriginDescription != null)
        //    query = query.Where(x => x.OriginDescription == criteria.OriginDescription);

        // if (criteria.OriginReason != null)
        //    query = query.Where(x => x.OriginReason == criteria.OriginReason);

        // if (criteria.OriginSource != null)
        //    query = query.Where(x => x.OriginSource == criteria.OriginSource);

        // if (criteria.UserId != null)
        //    query = query.Where(x => x.UserId == criteria.UserId);

        // if (criteria.OrganizationId != null)
        //    query = query.Where(x => x.OrganizationId == criteria.OrganizationId);

        // if (criteria.ProxyAgent != null)
        //    query = query.Where(x => x.ProxyAgent == criteria.ProxyAgent);

        // if (criteria.ProxySubject != null)
        //    query = query.Where(x => x.ProxySubject == criteria.ProxySubject);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}