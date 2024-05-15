using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Contract;

namespace Common.Sdk
{
    public class SdkClient
    {
        private readonly ApiClient _api;
        private readonly IApiClientFactory _apiClientFactory;

        public Pagination Pagination { get; private set; }

        public SdkClient(IApiClientFactory factory)
        {
            _api = new ApiClient(factory);
            _apiClientFactory = factory;
        }

        public async Task<string> Authenticate(int? lifetime = null)
            => await Authenticate(_apiClientFactory.GetSecret(), lifetime);

        public async Task<string> Authenticate(string secret, int? lifetime = null)
        {
            var client = _apiClientFactory.CreateClient();

            var provider = new ApiTokenProvider(client);

            var token = await provider.GetTokenAsync(client.BaseAddress, secret, lifetime);

            return token;
        }

        public async Task<T> Create<T>(string endpoint, object payload)
            => await _api.HttpPost<T>(endpoint, payload);

        public async Task Delete(string endpoint, Guid id)
            => await Delete(endpoint, id.ToString());

        public async Task Delete(string endpoint, Guid id1, Guid id2)
            => await Delete(endpoint, new[] { id1.ToString(), id2.ToString() });

        public async Task Delete(string endpoint, Guid id1, Guid id2, Guid id3)
            => await Delete(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task Delete(string endpoint, Guid id1, Guid id2, int id3)
            => await Delete(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task Delete(string endpoint, Guid id1, int id2)
            => await Delete(endpoint, new[] { id1.ToString(), id2.ToString() });

        public async Task Delete(string endpoint, Guid id1, string id2)
            => await Delete(endpoint, new[] { id1.ToString(), id2 });

        public async Task Delete(string endpoint, Guid id1, string id2, int id3)
            => await Delete(endpoint, new[] { id1.ToString(), id2, id3.ToString() });

        public async Task Delete(string endpoint, int id)
            => await Delete(endpoint, id.ToString());

        public async Task Delete(string endpoint, string id)
            => await _api.HttpDelete(endpoint, id);

        public async Task Delete(string endpoint, string[] id)
            => await _api.HttpDelete(endpoint, id);

        public async Task Export(string endpoint, object payload)
            => await _api.HttpPost(endpoint, payload);

        public async Task<T> Get<T>(string endpoint, string id)
            => await _api.HttpGet<T>(endpoint, id);

        public async Task<T> Get<T>(string endpoint, string[] id)
            => await _api.HttpGet<T>(endpoint, id);

        public async Task<T> Get<T>(string endpoint, Guid id)
            => await Get<T>(endpoint, id.ToString());

        public async Task<T> Get<T>(string endpoint, Guid id1, Guid id2)
            => await Get<T>(endpoint, new[] { id1.ToString(), id2.ToString() });

        public async Task<T> Get<T>(string endpoint, Guid id1, Guid id2, Guid id3)
            => await Get<T>(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task<T> Get<T>(string endpoint, Guid id1, Guid id2, int id3)
            => await Get<T>(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task<T> Get<T>(string endpoint, Guid id1, int id2)
            => await Get<T>(endpoint, new[] { id1.ToString(), id2.ToString() });

        public async Task<T> Get<T>(string endpoint, Guid id1, string id2)
            => await Get<T>(endpoint, new[] { id1.ToString(), id2 });

        public async Task<T> Get<T>(string endpoint, Guid id1, string id2, int id3)
            => await Get<T>(endpoint, new[] { id1.ToString(), id2, id3.ToString() });

        public async Task<T> Get<T>(string endpoint, int id)
            => await Get<T>(endpoint, id.ToString());

        public async Task<IEnumerable<T>> List<T>(string endpoint, Dictionary<string, string> criteria)
        {
            var many = await _api.HttpGet<T>(endpoint, criteria);
            Pagination = _api.Pagination;
            return many;
        }

        public async Task<T> Modify<T>(string endpoint, Guid id, object payload)
            => await _api.HttpPut<T>(endpoint, id.ToString(), payload);

        public async Task Modify(string endpoint, int id, object payload)
            => await _api.HttpPut(endpoint, id.ToString(), payload);

        public async Task Modify(string endpoint, string id, object payload)
            => await _api.HttpPut(endpoint, id, payload);

        public async Task Modify(string endpoint, Guid id, object payload)
            => await _api.HttpPut(endpoint, id.ToString(), payload);

        public async Task Modify(string endpoint, Guid id1, Guid id2, object payload)
            => await _api.HttpPut(endpoint, new[] { id1.ToString(), id2.ToString() }, payload);

        public async Task Modify(string endpoint, Guid id1, Guid id2, Guid id3, object payload)
            => await _api.HttpPut(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() }, payload);

        public async Task Modify(string endpoint, Guid id1, Guid id2, int id3, object payload)
            => await _api.HttpPut(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() }, payload);

        public async Task Modify(string endpoint, Guid id1, int id2, object payload)
            => await _api.HttpPut(endpoint, new[] { id1.ToString(), id2.ToString() }, payload);

        public async Task Modify(string endpoint, Guid id1, string id2, object payload)
            => await _api.HttpPut(endpoint, new[] { id1.ToString(), id2 }, payload);

        public async Task Modify(string endpoint, Guid id1, string id2, int id3, object payload)
            => await _api.HttpPut(endpoint, new[] { id1.ToString(), id2, id3.ToString() }, payload);

        public async Task<IEnumerable<T>> Search<T>(string endpoint, Dictionary<string, string> criteria)
        {
            var many = await _api.HttpGet<T>(endpoint, criteria);
            Pagination = _api.Pagination;
            return many;
        }

        public Dictionary<string, string> ToDictionary(object obj)
            => Utility.DictionaryConverter.ToDictionary(obj);
    }
}