using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Security;

public class PermissionService : IEntityService
{
    private readonly TPermissionReader _reader;
    private readonly TPermissionWriter _writer;

    private readonly TPermissionAdapter _adapter = new TPermissionAdapter();

    public PermissionService(TPermissionReader reader, TPermissionWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid permission, CancellationToken token)
        => await _reader.AssertAsync(permission, token);

    public async Task<PermissionModel?> FetchAsync(Guid permission, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(permission, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IPermissionCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<PermissionModel>> CollectAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<PermissionMatch>> SearchAsync(IPermissionCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreatePermission create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyPermission modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.PermissionId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid permission, CancellationToken token)
        => await _writer.DeleteAsync(permission, token);
}