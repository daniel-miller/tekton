using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class OrganizationClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateOrganization create)
            => await api.HttpPost(Endpoints.SecurityApi.Identification.Organization.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyOrganization modify)
            => await api.HttpPut(Endpoints.SecurityApi.Identification.Organization.Modify, modify.OrganizationId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteOrganization delete)
            => await api.HttpDelete(Endpoints.SecurityApi.Identification.Organization.Delete, delete.OrganizationId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid organization)
            => await api.Assert(Endpoints.SecurityApi.Identification.Organization.Assert, organization);

        public async Task<ApiResult<OrganizationModel>> FetchAsync(ApiClient api, Guid organization)
            => await api.HttpGet<OrganizationModel>(Endpoints.SecurityApi.Identification.Organization.Fetch, organization);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountOrganizations count)
            => await api.Count(Endpoints.SecurityApi.Identification.Organization.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<OrganizationModel>>> CollectAsync(ApiClient api, CollectOrganizations collect)
            => await api.HttpGet<IEnumerable<OrganizationModel>>(Endpoints.SecurityApi.Identification.Organization.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<OrganizationMatch>>> SearchAsync(ApiClient api, SearchOrganizations search)
            => await api.HttpGet<IEnumerable<OrganizationMatch>>(Endpoints.SecurityApi.Identification.Organization.Search, null, api.ToDictionary(search));
    }   
}