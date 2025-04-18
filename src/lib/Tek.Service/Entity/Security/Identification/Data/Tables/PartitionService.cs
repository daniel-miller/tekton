using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Security;

public class PartitionService : IEntityService
{
    private readonly TPartitionReader _reader;
    private readonly TPartitionWriter _writer;

    private readonly TPartitionAdapter _adapter = new TPartitionAdapter();

    public PartitionService(TPartitionReader reader, TPartitionWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(int partitionNumber, CancellationToken token)
        => await _reader.AssertAsync(partitionNumber, token);

    public async Task<PartitionModel?> FetchAsync(int partitionNumber, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(partitionNumber, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IPartitionCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<PartitionModel>> CollectAsync(IPartitionCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<PartitionMatch>> SearchAsync(IPartitionCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreatePartition create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyPartition modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.PartitionNumber, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(int partitionNumber, CancellationToken token)
        => await _writer.DeleteAsync(partitionNumber, token);
}