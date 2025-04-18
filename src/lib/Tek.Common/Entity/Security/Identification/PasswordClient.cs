using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class PasswordClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreatePassword create)
            => await api.HttpPost(Endpoints.SecurityApi.Identification.Password.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyPassword modify)
            => await api.HttpPut(Endpoints.SecurityApi.Identification.Password.Modify, modify.PasswordId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeletePassword delete)
            => await api.HttpDelete(Endpoints.SecurityApi.Identification.Password.Delete, delete.PasswordId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid password)
            => await api.Assert(Endpoints.SecurityApi.Identification.Password.Assert, password);

        public async Task<ApiResult<PasswordModel>> FetchAsync(ApiClient api, Guid password)
            => await api.HttpGet<PasswordModel>(Endpoints.SecurityApi.Identification.Password.Fetch, password);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountPasswords count)
            => await api.Count(Endpoints.SecurityApi.Identification.Password.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<PasswordModel>>> CollectAsync(ApiClient api, CollectPasswords collect)
            => await api.HttpGet<IEnumerable<PasswordModel>>(Endpoints.SecurityApi.Identification.Password.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<PasswordMatch>>> SearchAsync(ApiClient api, SearchPasswords search)
            => await api.HttpGet<IEnumerable<PasswordMatch>>(Endpoints.SecurityApi.Identification.Password.Search, null, api.ToDictionary(search));
    }   
}