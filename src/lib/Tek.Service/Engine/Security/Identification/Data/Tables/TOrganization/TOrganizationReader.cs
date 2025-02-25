using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TOrganizationReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<IOrganizationCriteria> _validator;
    private readonly OrganizationAdapter _adapter;

    public TOrganizationReader(IDbContextFactory<TableDbContext> context, 
        IValidator<IOrganizationCriteria> validator, OrganizationAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid organization, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TOrganization
            .AnyAsync(x => x.OrganizationId == organization, token);
    }

    public async Task<TOrganizationEntity?> FetchAsync(Guid organization, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TOrganization
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OrganizationId == organization, token);
    }

    public async Task<int> CountAsync(IOrganizationCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TOrganizationEntity>> CollectAsync(IOrganizationCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<OrganizationMatch>> SearchAsync(IOrganizationCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TOrganizationEntity> BuildQuery(IOrganizationCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TOrganization.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.OrganizationId != null)
        //    query = query.Where(x => x.OrganizationId == criteria.OrganizationId);

        // if (criteria.OrganizationNumber != null)
        //    query = query.Where(x => x.OrganizationNumber == criteria.OrganizationNumber);

        // if (criteria.OrganizationSlug != null)
        //    query = query.Where(x => x.OrganizationSlug == criteria.OrganizationSlug);

        // if (criteria.OrganizationName != null)
        //    query = query.Where(x => x.OrganizationName == criteria.OrganizationName);

        // if (criteria.PartitionNumber != null)
        //    query = query.Where(x => x.PartitionNumber == criteria.PartitionNumber);

        // if (criteria.ModifiedWhen != null)
        //    query = query.Where(x => x.ModifiedWhen == criteria.ModifiedWhen);

        return query;
    }
}