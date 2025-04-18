using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Bus;

public class AggregateService : IEntityService
{
    private readonly TAggregateReader _reader;
    private readonly TAggregateWriter _writer;

    private readonly TAggregateAdapter _adapter = new TAggregateAdapter();

    public AggregateService(TAggregateReader reader, TAggregateWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid aggregate, CancellationToken token)
        => await _reader.AssertAsync(aggregate, token);

    public async Task<AggregateModel?> FetchAsync(Guid aggregate, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(aggregate, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IAggregateCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<AggregateModel>> CollectAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<AggregateMatch>> SearchAsync(IAggregateCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateAggregate create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyAggregate modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.AggregateId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid aggregate, CancellationToken token)
        => await _writer.DeleteAsync(aggregate, token);
}