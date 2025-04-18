using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Bus;

public class TAggregateReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TAggregateAdapter _adapter;

    public TAggregateReader(IDbContextFactory<TableDbContext> context, TAggregateAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TAggregate
            .AnyAsync(x => x.AggregateId == aggregate, token);
    }

    public async Task<TAggregateEntity?> FetchAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TAggregate
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AggregateId == aggregate, token);
    }

    public async Task<int> CountAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TAggregateEntity>> CollectAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<AggregateMatch>> SearchAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TAggregateEntity> BuildQuery(TableDbContext db, IAggregateCriteria criteria)
    {
        var query = db.TAggregate.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.AggregateId != null)
        //    query = query.Where(x => x.AggregateId == criteria.AggregateId);

        // if (criteria.AggregateType != null)
        //    query = query.Where(x => x.AggregateType == criteria.AggregateType);

        // if (criteria.AggregateRoot != null)
        //    query = query.Where(x => x.AggregateRoot == criteria.AggregateRoot);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}