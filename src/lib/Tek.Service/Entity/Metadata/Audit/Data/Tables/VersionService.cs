using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Metadata;

public class VersionService : IEntityService
{
    private readonly TVersionReader _reader;
    private readonly TVersionWriter _writer;

    private readonly TVersionAdapter _adapter = new TVersionAdapter();

    public VersionService(TVersionReader reader, TVersionWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(int versionNumber, CancellationToken token)
        => await _reader.AssertAsync(versionNumber, token);

    public async Task<VersionModel?> FetchAsync(int versionNumber, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(versionNumber, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IVersionCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<VersionModel>> CollectAsync(IVersionCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<VersionMatch>> SearchAsync(IVersionCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateVersion create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyVersion modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.VersionNumber, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(int versionNumber, CancellationToken token)
        => await _writer.DeleteAsync(versionNumber, token);
}