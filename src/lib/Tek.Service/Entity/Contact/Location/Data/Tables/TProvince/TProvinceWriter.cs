using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Contact;

public class TProvinceWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TProvinceWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TProvinceEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.ProvinceId, token, db);
        if (exists)
            return false;
                
        await db.TProvince.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TProvinceEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.ProvinceId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid province, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TProvince.SingleOrDefaultAsync(x => x.ProvinceId == province, token);
        if (entity == null)
            return false;

        db.TProvince.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid province, CancellationToken token, TableDbContext db)
		=> await db.TProvince.AsNoTracking().AnyAsync(x => x.ProvinceId == province, token);
}