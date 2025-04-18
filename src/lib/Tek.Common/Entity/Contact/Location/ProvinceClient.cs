using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class ProvinceClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateProvince create)
            => await api.HttpPost(Endpoints.ContactApi.Location.Province.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyProvince modify)
            => await api.HttpPut(Endpoints.ContactApi.Location.Province.Modify, modify.ProvinceId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteProvince delete)
            => await api.HttpDelete(Endpoints.ContactApi.Location.Province.Delete, delete.ProvinceId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid province)
            => await api.Assert(Endpoints.ContactApi.Location.Province.Assert, province);

        public async Task<ApiResult<ProvinceModel>> FetchAsync(ApiClient api, Guid province)
            => await api.HttpGet<ProvinceModel>(Endpoints.ContactApi.Location.Province.Fetch, province);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountProvinces count)
            => await api.Count(Endpoints.ContactApi.Location.Province.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<ProvinceModel>>> CollectAsync(ApiClient api, CollectProvinces collect)
            => await api.HttpGet<IEnumerable<ProvinceModel>>(Endpoints.ContactApi.Location.Province.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<ProvinceMatch>>> SearchAsync(ApiClient api, SearchProvinces search)
            => await api.HttpGet<IEnumerable<ProvinceMatch>>(Endpoints.ContactApi.Location.Province.Search, null, api.ToDictionary(search));
    }   
}