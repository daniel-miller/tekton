using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TPasswordReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TPasswordAdapter _adapter;

    public TPasswordReader(IDbContextFactory<TableDbContext> context, TPasswordAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid password, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPassword
            .AnyAsync(x => x.PasswordId == password, token);
    }

    public async Task<TPasswordEntity?> FetchAsync(Guid password, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPassword
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PasswordId == password, token);
    }

    public async Task<int> CountAsync(IPasswordCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TPasswordEntity>> CollectAsync(IPasswordCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<PasswordMatch>> SearchAsync(IPasswordCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TPasswordEntity> BuildQuery(TableDbContext db, IPasswordCriteria criteria)
    {
        var query = db.TPassword.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.PasswordId != null)
        //    query = query.Where(x => x.PasswordId == criteria.PasswordId);

        // if (criteria.EmailId != null)
        //    query = query.Where(x => x.EmailId == criteria.EmailId);

        // if (criteria.EmailAddress != null)
        //    query = query.Where(x => x.EmailAddress == criteria.EmailAddress);

        // if (criteria.PasswordHash != null)
        //    query = query.Where(x => x.PasswordHash == criteria.PasswordHash);

        // if (criteria.PasswordExpiry != null)
        //    query = query.Where(x => x.PasswordExpiry == criteria.PasswordExpiry);

        // if (criteria.DefaultPlaintext != null)
        //    query = query.Where(x => x.DefaultPlaintext == criteria.DefaultPlaintext);

        // if (criteria.DefaultExpiry != null)
        //    query = query.Where(x => x.DefaultExpiry == criteria.DefaultExpiry);

        // if (criteria.CreatedWhen != null)
        //    query = query.Where(x => x.CreatedWhen == criteria.CreatedWhen);

        // if (criteria.LastForgottenWhen != null)
        //    query = query.Where(x => x.LastForgottenWhen == criteria.LastForgottenWhen);

        // if (criteria.LastModifiedWhen != null)
        //    query = query.Where(x => x.LastModifiedWhen == criteria.LastModifiedWhen);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}