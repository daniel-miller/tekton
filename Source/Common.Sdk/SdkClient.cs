using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Common.Contract;

namespace Common.Sdk
{
    public class SdkClient
    {
        private ApiClient _api;
        private IApiClientFactory _apiClientFactory;

        public Pagination Pagination { get; private set; }

        /// <remarks>
        /// Force callers to use the static Build method to instantiate a new SDK Client.
        /// </remarks>
        private SdkClient() { }

        /// <summary>
        /// Construct a new SdkClient
        /// </summary>
        /// <remarks></returns>
        static public async Task Build(IApiClientFactory factory)
        {
            var sdk = new SdkClient
            {
                _api = new ApiClient(factory),
                _apiClientFactory = factory
            };

            if (factory.GetToken() != null)
                return;

            var secret = factory.GetSecret();

            var token = await sdk.GetApiToken(secret);

            factory.SetToken(token);
        }

        public async Task<string> GetApiToken(string secret)
        {
            var client = _apiClientFactory.CreateClient();

            var provider = new ApiTokenProvider(client);
            
            var token = await provider.GetTokenAsync(client.BaseAddress, secret);
            
            return token;
        }

        public static Dictionary<string, string> ConvertToDictionary(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null.");

            Dictionary<string, string> result = new Dictionary<string, string>();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(obj);

                if (propertyValue != null)
                    result[propertyName] = propertyValue.ToString();
            }

            return result;
        }

        public async Task<T> Create<T>(string endpoint, object payload)
            => await _api.HttpPost<T>(endpoint, payload);

        public async Task Create(string endpoint, object payload)
            => await _api.HttpPost(endpoint, payload);

        public async Task Delete(string endpoint, Guid item)
            => await Delete(endpoint, item.ToString());

        public async Task Delete(string endpoint, Guid item1, Guid item2)
            => await Delete(endpoint, new[] { item1.ToString(), item2.ToString() });

        public async Task Delete(string endpoint, Guid item1, Guid item2, Guid item3)
            => await Delete(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() });

        public async Task Delete(string endpoint, Guid item1, Guid item2, int item3)
            => await Delete(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() });

        public async Task Delete(string endpoint, Guid item1, int item2)
            => await Delete(endpoint, new[] { item1.ToString(), item2.ToString() });

        public async Task Delete(string endpoint, Guid item1, string item2)
            => await Delete(endpoint, new[] { item1.ToString(), item2 });

        public async Task Delete(string endpoint, Guid item1, string item2, int item3)
            => await Delete(endpoint, new[] { item1.ToString(), item2, item3.ToString() });

        public async Task Delete(string endpoint, int item)
            => await Delete(endpoint, item.ToString());

        public async Task Delete(string endpoint, string item)
            => await _api.HttpDelete(endpoint, item);

        public async Task Delete(string endpoint, string[] item)
            => await _api.HttpDelete(endpoint, item);

        public async Task<T> GetItem<T>(string endpoint, string item)
            => await _api.HttpGet<T>(endpoint, item);

        public async Task<T> GetItem<T>(string endpoint, string[] item)
            => await _api.HttpGet<T>(endpoint, item);

        public async Task<T> GetItem<T>(string endpoint, Guid item)
            => await GetItem<T>(endpoint, item.ToString());

        public async Task<T> GetItem<T>(string endpoint, Guid item1, Guid item2)
            => await GetItem<T>(endpoint, new[] { item1.ToString(), item2.ToString() });

        public async Task<T> GetItem<T>(string endpoint, Guid item1, Guid item2, Guid item3)
            => await GetItem<T>(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() });

        public async Task<T> GetItem<T>(string endpoint, Guid item1, Guid item2, int item3)
            => await GetItem<T>(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() });

        public async Task<T> GetItem<T>(string endpoint, Guid item1, int item2)
            => await GetItem<T>(endpoint, new[] { item1.ToString(), item2.ToString() });

        public async Task<T> GetItem<T>(string endpoint, Guid item1, string item2)
            => await GetItem<T>(endpoint, new[] { item1.ToString(), item2 });

        public async Task<T> GetItem<T>(string endpoint, Guid item1, string item2, int item3)
            => await GetItem<T>(endpoint, new[] { item1.ToString(), item2, item3.ToString() });

        public async Task<T> GetItem<T>(string endpoint, int item)
            => await GetItem<T>(endpoint, item.ToString());

        public async Task<IEnumerable<T>> GetList<T>(string endpoint, Dictionary<string, string> variables)
        {
            var many = await _api.HttpGet<T>(endpoint, variables);
            Pagination = _api.Pagination;
            return many;
        }

        public async Task<T> Modify<T>(string endpoint, Guid item, object payload)
            => await _api.HttpPut<T>(endpoint, item.ToString(), payload);

        public async Task Modify(string endpoint, int item, object payload)
            => await _api.HttpPut(endpoint, item.ToString(), payload);

        public async Task Modify(string endpoint, string item, object payload)
            => await _api.HttpPut(endpoint, item, payload);

        public async Task Modify(string endpoint, Guid item, object payload)
            => await _api.HttpPut(endpoint, item.ToString(), payload);

        public async Task Modify(string endpoint, Guid item1, Guid item2, object payload)
            => await _api.HttpPut(endpoint, new[] { item1.ToString(), item2.ToString() }, payload);

        public async Task Modify(string endpoint, Guid item1, Guid item2, Guid item3, object payload)
            => await _api.HttpPut(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() }, payload);

        public async Task Modify(string endpoint, Guid item1, Guid item2, int item3, object payload)
            => await _api.HttpPut(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() }, payload);

        public async Task Modify(string endpoint, Guid item1, int item2, object payload)
            => await _api.HttpPut(endpoint, new[] { item1.ToString(), item2.ToString() }, payload);

        public async Task Modify(string endpoint, Guid item1, string item2, object payload)
            => await _api.HttpPut(endpoint, new[] { item1.ToString(), item2 }, payload);

        public async Task Modify(string endpoint, Guid item1, string item2, int item3, object payload)
            => await _api.HttpPut(endpoint, new[] { item1.ToString(), item2, item3.ToString() }, payload);
    }
}