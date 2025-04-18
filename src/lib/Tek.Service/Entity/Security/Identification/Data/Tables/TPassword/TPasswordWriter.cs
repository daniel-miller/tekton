using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TPasswordWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TPasswordWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TPasswordEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.PasswordId, token, db);
        if (exists)
            return false;
                
        await db.TPassword.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TPasswordEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.PasswordId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid password, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TPassword.SingleOrDefaultAsync(x => x.PasswordId == password, token);
        if (entity == null)
            return false;

        db.TPassword.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid password, CancellationToken token, TableDbContext db)
		=> await db.TPassword.AsNoTracking().AnyAsync(x => x.PasswordId == password, token);
}