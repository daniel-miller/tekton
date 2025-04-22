using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TRoleReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TRoleAdapter _adapter;

    public TRoleReader(IDbContextFactory<TableDbContext> context, TRoleAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid role, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TRole
            .AnyAsync(x => x.RoleId == role, token);
    }

    public async Task<TRoleEntity?> FetchAsync(Guid role, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TRole
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleId == role, token);
    }

    public async Task<int> CountAsync(IRoleCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TRoleEntity>> CollectAsync(IRoleCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<RoleMatch>> SearchAsync(IRoleCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TRoleEntity> BuildQuery(TableDbContext db, IRoleCriteria criteria)
    {
        var query = db.TRole.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.RoleId != null)
        //    query = query.Where(x => x.RoleId == criteria.RoleId);

        // if (criteria.RoleType != null)
        //    query = query.Where(x => x.RoleType == criteria.RoleType);

        // if (criteria.RoleName != null)
        //    query = query.Where(x => x.RoleName == criteria.RoleName);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}