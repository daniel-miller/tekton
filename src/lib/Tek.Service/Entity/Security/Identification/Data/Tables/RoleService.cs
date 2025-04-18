using Microsoft.EntityFrameworkCore;

using Tek.Contract;

namespace Tek.Service.Security;

public class RoleService : IEntityService
{
    private readonly TRoleReader _reader;
    private readonly TRoleWriter _writer;

    private readonly TRoleAdapter _adapter = new TRoleAdapter();

    public RoleService(TRoleReader reader, TRoleWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<bool> AssertAsync(Guid role, CancellationToken token)
        => await _reader.AssertAsync(role, token);

    public async Task<RoleModel?> FetchAsync(Guid role, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(role, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<int> CountAsync(IRoleCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<IEnumerable<RoleModel>> CollectAsync(IRoleCriteria criteria, CancellationToken token)
    {
        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<RoleMatch>> SearchAsync(IRoleCriteria criteria, CancellationToken token)
        => await _reader.SearchAsync(criteria, token);

    public async Task<bool> CreateAsync(CreateRole create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyRole modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.RoleId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid role, CancellationToken token)
        => await _writer.DeleteAsync(role, token);
}