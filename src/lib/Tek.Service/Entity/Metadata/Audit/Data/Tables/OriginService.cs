using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Metadata;

public class OriginService : IEntityService
{
    private readonly TOriginReader _reader;
    private readonly TOriginWriter _writer;

    private readonly TOriginAdapter _adapter = new TOriginAdapter();

    public OriginService(TOriginReader reader, TOriginWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid origin, CancellationToken token)
        => await _reader.AssertAsync(origin, token);

    public async Task<OriginModel?> FetchAsync(Guid origin, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(origin, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IOriginCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<OriginModel>> CollectAsync(IOriginCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<OriginMatch>> SearchAsync(IOriginCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateOrigin create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyOrigin modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.OriginId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid origin, CancellationToken token)
        => await _writer.DeleteAsync(origin, token);
}