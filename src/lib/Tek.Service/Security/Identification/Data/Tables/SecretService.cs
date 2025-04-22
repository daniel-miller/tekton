namespace Tek.Service.Security;

public class SecretService : IEntityService
{
    private readonly TSecretReader _reader;
    private readonly TSecretWriter _writer;

    private readonly TSecretAdapter _adapter = new TSecretAdapter();

    public SecretService(TSecretReader reader, TSecretWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid secret, CancellationToken token)
        => await _reader.AssertAsync(secret, token);

    public async Task<SecretModel?> FetchAsync(Guid secret, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(secret, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(ISecretCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<SecretModel>> CollectAsync(ISecretCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<SecretMatch>> SearchAsync(ISecretCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateSecret create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifySecret modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.SecretId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid secret, CancellationToken token)
        => await _writer.DeleteAsync(secret, token);
}