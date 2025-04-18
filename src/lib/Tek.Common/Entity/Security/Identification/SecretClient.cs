using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class SecretClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateSecret create)
            => await api.HttpPost(Endpoints.SecurityApi.Identification.Secret.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifySecret modify)
            => await api.HttpPut(Endpoints.SecurityApi.Identification.Secret.Modify, modify.SecretId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteSecret delete)
            => await api.HttpDelete(Endpoints.SecurityApi.Identification.Secret.Delete, delete.SecretId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid secret)
            => await api.Assert(Endpoints.SecurityApi.Identification.Secret.Assert, secret);

        public async Task<ApiResult<SecretModel>> FetchAsync(ApiClient api, Guid secret)
            => await api.HttpGet<SecretModel>(Endpoints.SecurityApi.Identification.Secret.Fetch, secret);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountSecrets count)
            => await api.Count(Endpoints.SecurityApi.Identification.Secret.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<SecretModel>>> CollectAsync(ApiClient api, CollectSecrets collect)
            => await api.HttpGet<IEnumerable<SecretModel>>(Endpoints.SecurityApi.Identification.Secret.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<SecretMatch>>> SearchAsync(ApiClient api, SearchSecrets search)
            => await api.HttpGet<IEnumerable<SecretMatch>>(Endpoints.SecurityApi.Identification.Secret.Search, null, api.ToDictionary(search));
    }   
}