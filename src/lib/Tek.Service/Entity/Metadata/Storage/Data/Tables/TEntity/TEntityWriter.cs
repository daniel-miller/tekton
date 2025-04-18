using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Metadata;

public class TEntityWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TEntityWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TEntityEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.EntityId, token, db);
        if (exists)
            return false;
                
        await db.TEntity.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TEntityEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.EntityId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var e = await db.TEntity.SingleOrDefaultAsync(x => x.EntityId == entity, token);
        if (e == null)
            return false;

        db.TEntity.Remove(e);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid entity, CancellationToken token, TableDbContext db)
		=> await db.TEntity.AsNoTracking().AnyAsync(x => x.EntityId == entity, token);
}