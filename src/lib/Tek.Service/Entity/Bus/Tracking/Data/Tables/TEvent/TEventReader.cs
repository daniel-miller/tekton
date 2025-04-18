using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Bus;

public class TEventReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TEventAdapter _adapter;

    public TEventReader(IDbContextFactory<TableDbContext> context, TEventAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid @event, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TEvent
            .AnyAsync(x => x.EventId == @event, token);
    }

    public async Task<TEventEntity?> FetchAsync(Guid @event, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TEvent
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EventId == @event, token);
    }

    public async Task<int> CountAsync(IEventCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TEventEntity>> CollectAsync(IEventCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<EventMatch>> SearchAsync(IEventCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TEventEntity> BuildQuery(TableDbContext db, IEventCriteria criteria)
    {
        var query = db.TEvent.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.EventId != null)
        //    query = query.Where(x => x.EventId == criteria.EventId);

        // if (criteria.EventType != null)
        //    query = query.Where(x => x.EventType == criteria.EventType);

        // if (criteria.EventData != null)
        //    query = query.Where(x => x.EventData == criteria.EventData);

        // if (criteria.AggregateId != null)
        //    query = query.Where(x => x.AggregateId == criteria.AggregateId);

        // if (criteria.AggregateVersion != null)
        //    query = query.Where(x => x.AggregateVersion == criteria.AggregateVersion);

        // if (criteria.OriginId != null)
        //    query = query.Where(x => x.OriginId == criteria.OriginId);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}