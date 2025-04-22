using Microsoft.EntityFrameworkCore;

namespace Tek.Service.Security;

public class TPartitionWriter : IEntityWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TPartitionWriter(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(TPartitionEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.PartitionNumber, token, db);
        if (exists)
            return false;
                
        await db.TPartition.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TPartitionEntity entity, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.PartitionNumber, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(int partitionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TPartition.SingleOrDefaultAsync(x => x.PartitionNumber == partitionNumber, token);
        if (entity == null)
            return false;

        db.TPartition.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(int partitionNumber, CancellationToken token, TableDbContext db)
		=> await db.TPartition.AsNoTracking().AnyAsync(x => x.PartitionNumber == partitionNumber, token);
}