using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class PartitionClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreatePartition create)
            => await api.HttpPost(Endpoints.SecurityApi.Identification.Partition.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyPartition modify)
            => await api.HttpPut(Endpoints.SecurityApi.Identification.Partition.Modify, modify.PartitionNumber, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeletePartition delete)
            => await api.HttpDelete(Endpoints.SecurityApi.Identification.Partition.Delete, delete.PartitionNumber);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, int partitionNumber)
            => await api.Assert(Endpoints.SecurityApi.Identification.Partition.Assert, partitionNumber);

        public async Task<ApiResult<PartitionModel>> FetchAsync(ApiClient api, int partitionNumber)
            => await api.HttpGet<PartitionModel>(Endpoints.SecurityApi.Identification.Partition.Fetch, partitionNumber);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountPartitions count)
            => await api.Count(Endpoints.SecurityApi.Identification.Partition.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<PartitionModel>>> CollectAsync(ApiClient api, CollectPartitions collect)
            => await api.HttpGet<IEnumerable<PartitionModel>>(Endpoints.SecurityApi.Identification.Partition.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<PartitionMatch>>> SearchAsync(ApiClient api, SearchPartitions search)
            => await api.HttpGet<IEnumerable<PartitionMatch>>(Endpoints.SecurityApi.Identification.Partition.Search, null, api.ToDictionary(search));
    }   
}