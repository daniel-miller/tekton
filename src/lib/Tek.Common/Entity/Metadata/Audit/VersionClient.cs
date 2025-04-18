using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class VersionClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateVersion create)
            => await api.HttpPost(Endpoints.MetadataApi.Audit.Version.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyVersion modify)
            => await api.HttpPut(Endpoints.MetadataApi.Audit.Version.Modify, modify.VersionNumber, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteVersion delete)
            => await api.HttpDelete(Endpoints.MetadataApi.Audit.Version.Delete, delete.VersionNumber);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, int versionNumber)
            => await api.Assert(Endpoints.MetadataApi.Audit.Version.Assert, versionNumber);

        public async Task<ApiResult<VersionModel>> FetchAsync(ApiClient api, int versionNumber)
            => await api.HttpGet<VersionModel>(Endpoints.MetadataApi.Audit.Version.Fetch, versionNumber);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountVersions count)
            => await api.Count(Endpoints.MetadataApi.Audit.Version.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<VersionModel>>> CollectAsync(ApiClient api, CollectVersions collect)
            => await api.HttpGet<IEnumerable<VersionModel>>(Endpoints.MetadataApi.Audit.Version.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<VersionMatch>>> SearchAsync(ApiClient api, SearchVersions search)
            => await api.HttpGet<IEnumerable<VersionMatch>>(Endpoints.MetadataApi.Audit.Version.Search, null, api.ToDictionary(search));
    }   
}