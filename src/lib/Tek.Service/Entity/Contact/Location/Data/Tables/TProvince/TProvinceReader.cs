using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Contact;

public class TProvinceReader : IEntityReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly TProvinceAdapter _adapter;

    public TProvinceReader(IDbContextFactory<TableDbContext> context, TProvinceAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid province, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TProvince
            .AnyAsync(x => x.ProvinceId == province, token);
    }

    public async Task<TProvinceEntity?> FetchAsync(Guid province, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TProvince
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProvinceId == province, token);
    }

    public async Task<int> CountAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TProvinceEntity>> CollectAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await BuildQuery(db, criteria)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<ProvinceMatch>> SearchAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entities = await BuildQuery(db, criteria)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TProvinceEntity> BuildQuery(TableDbContext db, IProvinceCriteria criteria)
    {
        var query = db.TProvince.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.ProvinceId != null)
        //    query = query.Where(x => x.ProvinceId == criteria.ProvinceId);

        // if (criteria.ProvinceCode != null)
        //    query = query.Where(x => x.ProvinceCode == criteria.ProvinceCode);

        // if (criteria.ProvinceName != null)
        //    query = query.Where(x => x.ProvinceName == criteria.ProvinceName);

        // if (criteria.CountryCode != null)
        //    query = query.Where(x => x.CountryCode == criteria.CountryCode);

        // if (criteria.CountryId != null)
        //    query = query.Where(x => x.CountryId == criteria.CountryId);

        if (criteria.Filter != null)
        {
            query = query
                .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
                .Take(criteria.Filter.Take);
        }

        return query;
    }
}