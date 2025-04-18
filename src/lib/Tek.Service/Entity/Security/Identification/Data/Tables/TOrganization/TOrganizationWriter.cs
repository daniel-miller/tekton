using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TOrganizationWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TOrganizationWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TOrganizationEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.OrganizationId, token, db);
        if (exists)
            return false;
                
        await db.TOrganization.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TOrganizationEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.OrganizationId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid organization, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TOrganization.SingleOrDefaultAsync(x => x.OrganizationId == organization, token);
        if (entity == null)
            return false;

        db.TOrganization.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid organization, CancellationToken token, TableDbContext db)
		=> await db.TOrganization.AsNoTracking().AnyAsync(x => x.OrganizationId == organization, token);
}