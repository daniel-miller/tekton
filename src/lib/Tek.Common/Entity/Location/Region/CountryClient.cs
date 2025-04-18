using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class CountryClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateCountry create)
            => await api.HttpPost(Endpoints.LocationApi.Region.Country.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyCountry modify)
            => await api.HttpPut(Endpoints.LocationApi.Region.Country.Modify, modify.CountryId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteCountry delete)
            => await api.HttpDelete(Endpoints.LocationApi.Region.Country.Delete, delete.CountryId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid country)
            => await api.Assert(Endpoints.LocationApi.Region.Country.Assert, country);

        public async Task<ApiResult<CountryModel>> FetchAsync(ApiClient api, Guid country)
            => await api.HttpGet<CountryModel>(Endpoints.LocationApi.Region.Country.Fetch, country);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountCountries count)
            => await api.Count(Endpoints.LocationApi.Region.Country.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<CountryModel>>> CollectAsync(ApiClient api, CollectCountries collect)
            => await api.HttpGet<IEnumerable<CountryModel>>(Endpoints.LocationApi.Region.Country.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<CountryMatch>>> SearchAsync(ApiClient api, SearchCountries search)
            => await api.HttpGet<IEnumerable<CountryMatch>>(Endpoints.LocationApi.Region.Country.Search, null, api.ToDictionary(search));
    }   
}