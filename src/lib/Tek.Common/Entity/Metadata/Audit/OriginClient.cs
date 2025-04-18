using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class OriginClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateOrigin create)
            => await api.HttpPost(Endpoints.MetadataApi.Audit.Origin.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyOrigin modify)
            => await api.HttpPut(Endpoints.MetadataApi.Audit.Origin.Modify, modify.OriginId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteOrigin delete)
            => await api.HttpDelete(Endpoints.MetadataApi.Audit.Origin.Delete, delete.OriginId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid origin)
            => await api.Assert(Endpoints.MetadataApi.Audit.Origin.Assert, origin);

        public async Task<ApiResult<OriginModel>> FetchAsync(ApiClient api, Guid origin)
            => await api.HttpGet<OriginModel>(Endpoints.MetadataApi.Audit.Origin.Fetch, origin);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountOrigins count)
            => await api.Count(Endpoints.MetadataApi.Audit.Origin.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<OriginModel>>> CollectAsync(ApiClient api, CollectOrigins collect)
            => await api.HttpGet<IEnumerable<OriginModel>>(Endpoints.MetadataApi.Audit.Origin.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<OriginMatch>>> SearchAsync(ApiClient api, SearchOrigins search)
            => await api.HttpGet<IEnumerable<OriginMatch>>(Endpoints.MetadataApi.Audit.Origin.Search, null, api.ToDictionary(search));
    }   
}