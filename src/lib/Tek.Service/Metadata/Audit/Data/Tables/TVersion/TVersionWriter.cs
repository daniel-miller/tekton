using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Metadata;

public class TVersionWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TVersionWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TVersionEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.VersionNumber, token, db);
        if (exists)
            return false;
                
        await db.TVersion.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TVersionEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.VersionNumber, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(int versionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TVersion.SingleOrDefaultAsync(x => x.VersionNumber == versionNumber, token);
        if (entity == null)
            return false;

        db.TVersion.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(int versionNumber, CancellationToken token, TableDbContext db)
		=> await db.TVersion.AsNoTracking().AnyAsync(x => x.VersionNumber == versionNumber, token);
}