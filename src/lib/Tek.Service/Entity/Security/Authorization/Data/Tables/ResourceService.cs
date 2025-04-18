using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Security;

public class ResourceService : IEntityService
{
    private readonly TResourceReader _reader;
    private readonly TResourceWriter _writer;

    private readonly TResourceAdapter _adapter = new TResourceAdapter();

    public ResourceService(TResourceReader reader, TResourceWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid resource, CancellationToken token)
        => await _reader.AssertAsync(resource, token);

    public async Task<ResourceModel?> FetchAsync(Guid resource, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(resource, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IResourceCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<ResourceModel>> CollectAsync(IResourceCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<ResourceMatch>> SearchAsync(IResourceCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateResource create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyResource modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.ResourceId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid resource, CancellationToken token)
        => await _writer.DeleteAsync(resource, token);
}