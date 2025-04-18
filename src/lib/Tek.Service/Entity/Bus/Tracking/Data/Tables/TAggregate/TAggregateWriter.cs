using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Bus;

public class TAggregateWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TAggregateWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TAggregateEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.AggregateId, token, db);
        if (exists)
            return false;
                
        await db.TAggregate.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TAggregateEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.AggregateId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TAggregate.SingleOrDefaultAsync(x => x.AggregateId == aggregate, token);
        if (entity == null)
            return false;

        db.TAggregate.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid aggregate, CancellationToken token, TableDbContext db)
		=> await db.TAggregate.AsNoTracking().AnyAsync(x => x.AggregateId == aggregate, token);
}