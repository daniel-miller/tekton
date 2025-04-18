using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class EntityClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateEntity create)
            => await api.HttpPost(Endpoints.MetadataApi.Storage.Entity.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyEntity modify)
            => await api.HttpPut(Endpoints.MetadataApi.Storage.Entity.Modify, modify.EntityId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteEntity delete)
            => await api.HttpDelete(Endpoints.MetadataApi.Storage.Entity.Delete, delete.EntityId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid entity)
            => await api.Assert(Endpoints.MetadataApi.Storage.Entity.Assert, entity);

        public async Task<ApiResult<EntityModel>> FetchAsync(ApiClient api, Guid entity)
            => await api.HttpGet<EntityModel>(Endpoints.MetadataApi.Storage.Entity.Fetch, entity);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountEntities count)
            => await api.Count(Endpoints.MetadataApi.Storage.Entity.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<EntityModel>>> CollectAsync(ApiClient api, CollectEntities collect)
            => await api.HttpGet<IEnumerable<EntityModel>>(Endpoints.MetadataApi.Storage.Entity.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<EntityMatch>>> SearchAsync(ApiClient api, SearchEntities search)
            => await api.HttpGet<IEnumerable<EntityMatch>>(Endpoints.MetadataApi.Storage.Entity.Search, null, api.ToDictionary(search));
    }   
}