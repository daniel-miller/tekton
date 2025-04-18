using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class ResourceClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateResource create)
            => await api.HttpPost(Endpoints.SecurityApi.Authorization.Resource.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyResource modify)
            => await api.HttpPut(Endpoints.SecurityApi.Authorization.Resource.Modify, modify.ResourceId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteResource delete)
            => await api.HttpDelete(Endpoints.SecurityApi.Authorization.Resource.Delete, delete.ResourceId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid resource)
            => await api.Assert(Endpoints.SecurityApi.Authorization.Resource.Assert, resource);

        public async Task<ApiResult<ResourceModel>> FetchAsync(ApiClient api, Guid resource)
            => await api.HttpGet<ResourceModel>(Endpoints.SecurityApi.Authorization.Resource.Fetch, resource);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountResources count)
            => await api.Count(Endpoints.SecurityApi.Authorization.Resource.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<ResourceModel>>> CollectAsync(ApiClient api, CollectResources collect)
            => await api.HttpGet<IEnumerable<ResourceModel>>(Endpoints.SecurityApi.Authorization.Resource.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<ResourceMatch>>> SearchAsync(ApiClient api, SearchResources search)
            => await api.HttpGet<IEnumerable<ResourceMatch>>(Endpoints.SecurityApi.Authorization.Resource.Search, null, api.ToDictionary(search));
    }   
}