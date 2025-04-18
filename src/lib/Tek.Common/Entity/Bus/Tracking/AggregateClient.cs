using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class AggregateClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateAggregate create)
            => await api.HttpPost(Endpoints.BusApi.Tracking.Aggregate.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyAggregate modify)
            => await api.HttpPut(Endpoints.BusApi.Tracking.Aggregate.Modify, modify.AggregateId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteAggregate delete)
            => await api.HttpDelete(Endpoints.BusApi.Tracking.Aggregate.Delete, delete.AggregateId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid aggregate)
            => await api.Assert(Endpoints.BusApi.Tracking.Aggregate.Assert, aggregate);

        public async Task<ApiResult<AggregateModel>> FetchAsync(ApiClient api, Guid aggregate)
            => await api.HttpGet<AggregateModel>(Endpoints.BusApi.Tracking.Aggregate.Fetch, aggregate);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountAggregates count)
            => await api.Count(Endpoints.BusApi.Tracking.Aggregate.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<AggregateModel>>> CollectAsync(ApiClient api, CollectAggregates collect)
            => await api.HttpGet<IEnumerable<AggregateModel>>(Endpoints.BusApi.Tracking.Aggregate.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<AggregateMatch>>> SearchAsync(ApiClient api, SearchAggregates search)
            => await api.HttpGet<IEnumerable<AggregateMatch>>(Endpoints.BusApi.Tracking.Aggregate.Search, null, api.ToDictionary(search));
    }   
}