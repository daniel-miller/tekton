using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class PermissionClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreatePermission create)
            => await api.HttpPost(Endpoints.SecurityApi.Authorization.Permission.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyPermission modify)
            => await api.HttpPut(Endpoints.SecurityApi.Authorization.Permission.Modify, modify.PermissionId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeletePermission delete)
            => await api.HttpDelete(Endpoints.SecurityApi.Authorization.Permission.Delete, delete.PermissionId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid permission)
            => await api.Assert(Endpoints.SecurityApi.Authorization.Permission.Assert, permission);

        public async Task<ApiResult<PermissionModel>> FetchAsync(ApiClient api, Guid permission)
            => await api.HttpGet<PermissionModel>(Endpoints.SecurityApi.Authorization.Permission.Fetch, permission);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountPermissions count)
            => await api.Count(Endpoints.SecurityApi.Authorization.Permission.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<PermissionModel>>> CollectAsync(ApiClient api, CollectPermissions collect)
            => await api.HttpGet<IEnumerable<PermissionModel>>(Endpoints.SecurityApi.Authorization.Permission.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<PermissionMatch>>> SearchAsync(ApiClient api, SearchPermissions search)
            => await api.HttpGet<IEnumerable<PermissionMatch>>(Endpoints.SecurityApi.Authorization.Permission.Search, null, api.ToDictionary(search));
    }   
}