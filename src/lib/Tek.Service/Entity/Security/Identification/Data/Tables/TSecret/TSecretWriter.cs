using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TSecretWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TSecretWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TSecretEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.SecretId, token, db);
        if (exists)
            return false;
                
        await db.TSecret.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TSecretEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.SecretId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid secret, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TSecret.SingleOrDefaultAsync(x => x.SecretId == secret, token);
        if (entity == null)
            return false;

        db.TSecret.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid secret, CancellationToken token, TableDbContext db)
		=> await db.TSecret.AsNoTracking().AnyAsync(x => x.SecretId == secret, token);
}