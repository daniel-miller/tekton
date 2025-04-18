using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Metadata;

public class TEntityReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TEntityAdapter _adapter;

    public TEntityReader(IDbContextFactory<TableDbContext> context, TEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TEntity
            .AnyAsync(x => x.EntityId == entity, token);
    }

    public async Task<TEntityEntity?> FetchAsync(Guid entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TEntity
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EntityId == entity, token);
    }

    public async Task<int> CountAsync(IEntityCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TEntityEntity>> CollectAsync(IEntityCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<EntityMatch>> SearchAsync(IEntityCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TEntityEntity> BuildQuery(TableDbContext db, IEntityCriteria criteria)
    {
        var query = db.TEntity.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.StorageStructure != null)
        //    query = query.Where(x => x.StorageStructure == criteria.StorageStructure);

        // if (criteria.StorageSchema != null)
        //    query = query.Where(x => x.StorageSchema == criteria.StorageSchema);

        // if (criteria.StorageTable != null)
        //    query = query.Where(x => x.StorageTable == criteria.StorageTable);

        // if (criteria.StorageTableFuture != null)
        //    query = query.Where(x => x.StorageTableFuture == criteria.StorageTableFuture);

        // if (criteria.StorageKey != null)
        //    query = query.Where(x => x.StorageKey == criteria.StorageKey);

        // if (criteria.ComponentType != null)
        //    query = query.Where(x => x.ComponentType == criteria.ComponentType);

        // if (criteria.ComponentName != null)
        //    query = query.Where(x => x.ComponentName == criteria.ComponentName);

        // if (criteria.ComponentFeature != null)
        //    query = query.Where(x => x.ComponentFeature == criteria.ComponentFeature);

        // if (criteria.EntityName != null)
        //    query = query.Where(x => x.EntityName == criteria.EntityName);

        // if (criteria.EntityId != null)
        //    query = query.Where(x => x.EntityId == criteria.EntityId);

        // if (criteria.CollectionSlug != null)
        //    query = query.Where(x => x.CollectionSlug == criteria.CollectionSlug);

        // if (criteria.CollectionKey != null)
        //    query = query.Where(x => x.CollectionKey == criteria.CollectionKey);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}