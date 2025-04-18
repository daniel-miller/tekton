using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Metadata;

public class EntityService : IEntityService
{
    private readonly TEntityReader _reader;
    private readonly TEntityWriter _writer;

    private readonly TEntityAdapter _adapter = new TEntityAdapter();

    public EntityService(TEntityReader reader, TEntityWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid entity, CancellationToken token)
        => await _reader.AssertAsync(entity, token);

    public async Task<EntityModel?> FetchAsync(Guid entity, CancellationToken token)
    {
        var e = await _reader.FetchAsync(entity, token);

        return e != null ? _adapter.ToModel(e) : null;
    }

    public async Task<int> CountAsync(IEntityCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<EntityModel>> CollectAsync(IEntityCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<EntityMatch>> SearchAsync(IEntityCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateEntity create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyEntity modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.EntityId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid entity, CancellationToken token)
        => await _writer.DeleteAsync(entity, token);
}