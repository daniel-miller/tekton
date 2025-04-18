using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class EventClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateEvent create)
            => await api.HttpPost(Endpoints.BusApi.Tracking.Event.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyEvent modify)
            => await api.HttpPut(Endpoints.BusApi.Tracking.Event.Modify, modify.EventId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteEvent delete)
            => await api.HttpDelete(Endpoints.BusApi.Tracking.Event.Delete, delete.EventId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid @event)
            => await api.Assert(Endpoints.BusApi.Tracking.Event.Assert, @event);

        public async Task<ApiResult<EventModel>> FetchAsync(ApiClient api, Guid @event)
            => await api.HttpGet<EventModel>(Endpoints.BusApi.Tracking.Event.Fetch, @event);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountEvents count)
            => await api.Count(Endpoints.BusApi.Tracking.Event.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<EventModel>>> CollectAsync(ApiClient api, CollectEvents collect)
            => await api.HttpGet<IEnumerable<EventModel>>(Endpoints.BusApi.Tracking.Event.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<EventMatch>>> SearchAsync(ApiClient api, SearchEvents search)
            => await api.HttpGet<IEnumerable<EventMatch>>(Endpoints.BusApi.Tracking.Event.Search, null, api.ToDictionary(search));
    }   
}