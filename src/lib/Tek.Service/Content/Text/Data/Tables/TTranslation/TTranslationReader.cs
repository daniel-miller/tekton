using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Content;

public class TTranslationReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TTranslationAdapter _adapter;

    public TTranslationReader(IDbContextFactory<TableDbContext> context, TTranslationAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid translation, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TTranslation
            .AnyAsync(x => x.TranslationId == translation, token);
    }

    public async Task<TTranslationEntity?> FetchAsync(Guid translation, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TTranslation
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TranslationId == translation, token);
    }

    public async Task<int> CountAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TTranslationEntity>> CollectAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<TranslationMatch>> SearchAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TTranslationEntity> BuildQuery(TableDbContext db, ITranslationCriteria criteria)
    {
        var query = db.TTranslation.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.TranslationId != null)
        //    query = query.Where(x => x.TranslationId == criteria.TranslationId);

        // if (criteria.TranslationText != null)
        //    query = query.Where(x => x.TranslationText == criteria.TranslationText);

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