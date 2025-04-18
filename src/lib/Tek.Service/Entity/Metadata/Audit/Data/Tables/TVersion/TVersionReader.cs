using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Metadata;

public class TVersionReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TVersionAdapter _adapter;

    public TVersionReader(IDbContextFactory<TableDbContext> context, TVersionAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(int versionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TVersion
            .AnyAsync(x => x.VersionNumber == versionNumber, token);
    }

    public async Task<TVersionEntity?> FetchAsync(int versionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TVersion
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.VersionNumber == versionNumber, token);
    }

    public async Task<int> CountAsync(IVersionCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TVersionEntity>> CollectAsync(IVersionCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<VersionMatch>> SearchAsync(IVersionCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TVersionEntity> BuildQuery(TableDbContext db, IVersionCriteria criteria)
    {
        var query = db.TVersion.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.VersionNumber != null)
        //    query = query.Where(x => x.VersionNumber == criteria.VersionNumber);

        // if (criteria.VersionType != null)
        //    query = query.Where(x => x.VersionType == criteria.VersionType);

        // if (criteria.VersionName != null)
        //    query = query.Where(x => x.VersionName == criteria.VersionName);

        // if (criteria.ScriptPath != null)
        //    query = query.Where(x => x.ScriptPath == criteria.ScriptPath);

        // if (criteria.ScriptContent != null)
        //    query = query.Where(x => x.ScriptContent == criteria.ScriptContent);

        // if (criteria.ScriptExecuted != null)
        //    query = query.Where(x => x.ScriptExecuted == criteria.ScriptExecuted);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}