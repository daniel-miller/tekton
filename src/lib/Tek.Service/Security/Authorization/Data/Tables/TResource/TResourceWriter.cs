using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TResourceWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TResourceWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TResourceEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.ResourceId, token, db);
        if (exists)
            return false;
                
        await db.TResource.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TResourceEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.ResourceId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid resource, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TResource.SingleOrDefaultAsync(x => x.ResourceId == resource, token);
        if (entity == null)
            return false;

        db.TResource.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid resource, CancellationToken token, TableDbContext db)
		=> await db.TResource.AsNoTracking().AnyAsync(x => x.ResourceId == resource, token);
}