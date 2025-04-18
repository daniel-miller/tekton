using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TRoleWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TRoleWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TRoleEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.RoleId, token, db);
        if (exists)
            return false;
                
        await db.TRole.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TRoleEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.RoleId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid role, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TRole.SingleOrDefaultAsync(x => x.RoleId == role, token);
        if (entity == null)
            return false;

        db.TRole.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid role, CancellationToken token, TableDbContext db)
		=> await db.TRole.AsNoTracking().AnyAsync(x => x.RoleId == role, token);
}