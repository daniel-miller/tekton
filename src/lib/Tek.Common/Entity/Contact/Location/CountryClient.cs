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
            => await api.HttpPost(Endpoints.ContactApi.Location.Country.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyCountry modify)
            => await api.HttpPut(Endpoints.ContactApi.Location.Country.Modify, modify.CountryId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteCountry delete)
            => await api.HttpDelete(Endpoints.ContactApi.Location.Country.Delete, delete.CountryId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid country)
            => await api.Assert(Endpoints.ContactApi.Location.Country.Assert, country);

        public async Task<ApiResult<CountryModel>> FetchAsync(ApiClient api, Guid country)
            => await api.HttpGet<CountryModel>(Endpoints.ContactApi.Location.Country.Fetch, country);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountCountries count)
            => await api.Count(Endpoints.ContactApi.Location.Country.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<CountryModel>>> CollectAsync(ApiClient api, CollectCountries collect)
            => await api.HttpGet<IEnumerable<CountryModel>>(Endpoints.ContactApi.Location.Country.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<CountryMatch>>> SearchAsync(ApiClient api, SearchCountries search)
            => await api.HttpGet<IEnumerable<CountryMatch>>(Endpoints.ContactApi.Location.Country.Search, null, api.ToDictionary(search));
    }   
}