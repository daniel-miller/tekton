using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TSecretReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TSecretAdapter _adapter;

    public TSecretReader(IDbContextFactory<TableDbContext> context, TSecretAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid secret, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TSecret
            .AnyAsync(x => x.SecretId == secret, token);
    }

    public async Task<TSecretEntity?> FetchAsync(Guid secret, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TSecret
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SecretId == secret, token);
    }

    public async Task<int> CountAsync(ISecretCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TSecretEntity>> CollectAsync(ISecretCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<SecretMatch>> SearchAsync(ISecretCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TSecretEntity> BuildQuery(TableDbContext db, ISecretCriteria criteria)
    {
        var query = db.TSecret.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.PasswordId != null)
        //    query = query.Where(x => x.PasswordId == criteria.PasswordId);

        // if (criteria.SecretId != null)
        //    query = query.Where(x => x.SecretId == criteria.SecretId);

        // if (criteria.SecretType != null)
        //    query = query.Where(x => x.SecretType == criteria.SecretType);

        // if (criteria.SecretName != null)
        //    query = query.Where(x => x.SecretName == criteria.SecretName);

        // if (criteria.SecretValue != null)
        //    query = query.Where(x => x.SecretValue == criteria.SecretValue);

        // if (criteria.SecretScope != null)
        //    query = query.Where(x => x.SecretScope == criteria.SecretScope);

        // if (criteria.SecretExpiry != null)
        //    query = query.Where(x => x.SecretExpiry == criteria.SecretExpiry);

        // if (criteria.SecretLimetimeLimit != null)
        //    query = query.Where(x => x.SecretLimetimeLimit == criteria.SecretLimetimeLimit);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}