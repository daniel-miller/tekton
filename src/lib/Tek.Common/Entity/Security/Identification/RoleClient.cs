using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class RoleClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateRole create)
            => await api.HttpPost(Endpoints.SecurityApi.Identification.Role.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyRole modify)
            => await api.HttpPut(Endpoints.SecurityApi.Identification.Role.Modify, modify.RoleId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteRole delete)
            => await api.HttpDelete(Endpoints.SecurityApi.Identification.Role.Delete, delete.RoleId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid role)
            => await api.Assert(Endpoints.SecurityApi.Identification.Role.Assert, role);

        public async Task<ApiResult<RoleModel>> FetchAsync(ApiClient api, Guid role)
            => await api.HttpGet<RoleModel>(Endpoints.SecurityApi.Identification.Role.Fetch, role);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountRoles count)
            => await api.Count(Endpoints.SecurityApi.Identification.Role.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<RoleModel>>> CollectAsync(ApiClient api, CollectRoles collect)
            => await api.HttpGet<IEnumerable<RoleModel>>(Endpoints.SecurityApi.Identification.Role.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<RoleMatch>>> SearchAsync(ApiClient api, SearchRoles search)
            => await api.HttpGet<IEnumerable<RoleMatch>>(Endpoints.SecurityApi.Identification.Role.Search, null, api.ToDictionary(search));
    }   
}