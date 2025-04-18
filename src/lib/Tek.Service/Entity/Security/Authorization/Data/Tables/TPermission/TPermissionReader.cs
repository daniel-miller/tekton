using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TPermissionReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TPermissionAdapter _adapter;

    public TPermissionReader(IDbContextFactory<TableDbContext> context, TPermissionAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid permission, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPermission
            .AnyAsync(x => x.PermissionId == permission, token);
    }

    public async Task<TPermissionEntity?> FetchAsync(Guid permission, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPermission
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PermissionId == permission, token);
    }

    public async Task<int> CountAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TPermissionEntity>> CollectAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<PermissionMatch>> SearchAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TPermissionEntity> BuildQuery(TableDbContext db, IPermissionCriteria criteria)
    {
        var query = db.TPermission.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.PermissionId != null)
        //    query = query.Where(x => x.PermissionId == criteria.PermissionId);

        // if (criteria.AccessType != null)
        //    query = query.Where(x => x.AccessType == criteria.AccessType);

        // if (criteria.AccessFlags != null)
        //    query = query.Where(x => x.AccessFlags == criteria.AccessFlags);

        // if (criteria.ResourceId != null)
        //    query = query.Where(x => x.ResourceId == criteria.ResourceId);

        // if (criteria.RoleId != null)
        //    query = query.Where(x => x.RoleId == criteria.RoleId);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}