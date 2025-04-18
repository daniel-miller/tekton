using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tek.Contract;

namespace Tek.Common
{
    public class TranslationClient
    {
        // Commands

        public async Task<ApiResult> CreateAsync(ApiClient api, CreateTranslation create)
            => await api.HttpPost(Endpoints.ContentApi.Text.Translation.Create, create);

        public async Task<ApiResult> ModifyAsync(ApiClient api, ModifyTranslation modify)
            => await api.HttpPut(Endpoints.ContentApi.Text.Translation.Modify, modify.TranslationId, modify);

        public async Task<ApiResult> DeleteAsync(ApiClient api, DeleteTranslation delete)
            => await api.HttpDelete(Endpoints.ContentApi.Text.Translation.Delete, delete.TranslationId);

        // Queries

        public async Task<ApiResult<bool>> AssertAsync(ApiClient api, Guid translation)
            => await api.Assert(Endpoints.ContentApi.Text.Translation.Assert, translation);

        public async Task<ApiResult<TranslationModel>> FetchAsync(ApiClient api, Guid translation)
            => await api.HttpGet<TranslationModel>(Endpoints.ContentApi.Text.Translation.Fetch, translation);

        public async Task<ApiResult<int>> CountAsync(ApiClient api, CountTranslations count)
            => await api.Count(Endpoints.ContentApi.Text.Translation.Count, api.ToDictionary(count));

        public async Task<ApiResult<IEnumerable<TranslationModel>>> CollectAsync(ApiClient api, CollectTranslations collect)
            => await api.HttpGet<IEnumerable<TranslationModel>>(Endpoints.ContentApi.Text.Translation.Collect, null, api.ToDictionary(collect));

        public async Task<ApiResult<IEnumerable<TranslationMatch>>> SearchAsync(ApiClient api, SearchTranslations search)
            => await api.HttpGet<IEnumerable<TranslationMatch>>(Endpoints.ContentApi.Text.Translation.Search, null, api.ToDictionary(search));
    }   
}