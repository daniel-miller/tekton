using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Security;

public class OrganizationService : IEntityService
{
    private readonly TOrganizationReader _reader;
    private readonly TOrganizationWriter _writer;

    private readonly TOrganizationAdapter _adapter = new TOrganizationAdapter();

    public OrganizationService(TOrganizationReader reader, TOrganizationWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid organization, CancellationToken token)
        => await _reader.AssertAsync(organization, token);

    public async Task<OrganizationModel?> FetchAsync(Guid organization, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(organization, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IOrganizationCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<OrganizationModel>> CollectAsync(IOrganizationCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<OrganizationMatch>> SearchAsync(IOrganizationCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateOrganization create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyOrganization modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.OrganizationId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid organization, CancellationToken token)
        => await _writer.DeleteAsync(organization, token);
}