using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Contact;

public class CountryService : IEntityService
{
    private readonly TCountryReader _reader;
    private readonly TCountryWriter _writer;

    private readonly TCountryAdapter _adapter = new TCountryAdapter();

    public CountryService(TCountryReader reader, TCountryWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid country, CancellationToken token)
        => await _reader.AssertAsync(country, token);

    public async Task<CountryModel?> FetchAsync(Guid country, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(country, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(ICountryCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<CountryModel>> CollectAsync(ICountryCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<CountryMatch>> SearchAsync(ICountryCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateCountry create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyCountry modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.CountryId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid country, CancellationToken token)
        => await _writer.DeleteAsync(country, token);
}