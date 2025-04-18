using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Bus;

public class TEventWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TEventWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TEventEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.EventId, token, db);
        if (exists)
            return false;
                
        await db.TEvent.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TEventEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.EventId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid @event, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TEvent.SingleOrDefaultAsync(x => x.EventId == @event, token);
        if (entity == null)
            return false;

        db.TEvent.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid @event, CancellationToken token, TableDbContext db)
		=> await db.TEvent.AsNoTracking().AnyAsync(x => x.EventId == @event, token);
}