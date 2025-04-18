using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Contact;

public class ProvinceService : IEntityService
{
    private readonly TProvinceReader _reader;
    private readonly TProvinceWriter _writer;

    private readonly TProvinceAdapter _adapter = new TProvinceAdapter();

    public ProvinceService(TProvinceReader reader, TProvinceWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid province, CancellationToken token)
        => await _reader.AssertAsync(province, token);

    public async Task<ProvinceModel?> FetchAsync(Guid province, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(province, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IProvinceCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<ProvinceModel>> CollectAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<ProvinceMatch>> SearchAsync(IProvinceCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateProvince create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyProvince modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.ProvinceId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid province, CancellationToken token)
        => await _writer.DeleteAsync(province, token);
}