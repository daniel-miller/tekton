using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common
{
    public class ApiClient
    {
        protected readonly IApiClientFactory _factory;

        public Pagination Pagination { get; private set; }

        #region Construction

        public ApiClient(IApiClientFactory apiClientFactory)
        {
            _factory = apiClientFactory;
        }

        private HttpClient CreateHttpClient()
        {
            var client = _factory.CreateClient();
            var token = _factory.GetToken();
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public Dictionary<string, string> ToDictionary(object criteria)
            => DictionaryConverter.ToDictionary(criteria);

        #endregion

        public async Task<string> Authenticate(int? lifetime = null)
            => await Authenticate(_factory.GetSecret(), lifetime);

        public async Task<string> Authenticate(string secret, int? lifetime = null)
        {
            var client = _factory.CreateClient();

            var provider = new ApiTokenProvider(client);

            var token = await provider.GetTokenAsync(client.BaseAddress, secret, lifetime);

            return token;
        }

        public async Task<TResult> ExecuteCommand<TResult>(IQuery<TResult> query)
            => throw new NotImplementedException();

        public async Task<TResult> RunQuery<TResult>(IQuery<TResult> query)
            => throw new NotImplementedException();

        public async Task<TResult> PublishEvent<TResult>(IQuery<TResult> query)
            => throw new NotImplementedException();

        #region HTTP Requests (GET, POST, PUT, DELETE)

        public async Task<T> HttpGet<T>(string endpoint, string[] id)
        {
            var url = endpoint.ToString();
            if (id != null && id.Length > 0)
                url += "/" + string.Join("/", id);

            var client = CreateHttpClient();
            var http = await client.GetAsync(url);
            var json = await http.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
        }

        public async Task<T> HttpGet<T>(string endpoint, Dictionary<string, string> criteria)
        {
            var url = endpoint.ToString() + "?";
            foreach (var kvp in criteria)
                url += $"{kvp.Key}={kvp.Value}&";

            var client = CreateHttpClient();
            var http = await client.GetAsync(url);
            var json = await http.Content.ReadAsStringAsync();

            if (http.StatusCode != HttpStatusCode.OK)
                throw new ApiException($"An unexpected HTTP response was received from {endpoint}: {(int)http.StatusCode} {http.StatusCode}. {http.RequestMessage}");

            try
            {
                var response = JsonSerializer.Deserialize<T>(json);

                if (http.Headers.TryGetValues(Pagination.HeaderKey, out IEnumerable<string> values))
                    Pagination = JsonSerializer.Deserialize<Pagination>(values.First());

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected JSON deserialization error occurred on the response received from {endpoint}. {ex.Message}. JSON = {json}");
            }
        }

        public async Task<T> HttpGet<T>(string endpoint, string id)
            => await HttpGet<T>(endpoint, new[] { id });

        public async Task<T> HttpGet<T>(string endpoint, Guid id)
            => await HttpGet<T>(endpoint, id.ToString());

        public async Task<T> HttpGet<T>(string endpoint, Guid id1, Guid id2)
            => await HttpGet<T>(endpoint, new[] { id1.ToString(), id2.ToString() });

        public async Task<T> HttpGet<T>(string endpoint, Guid id1, Guid id2, Guid id3)
            => await HttpGet<T>(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task<T> HttpGet<T>(string endpoint, Guid id1, Guid id2, int id3)
            => await HttpGet<T>(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task<T> HttpGet<T>(string endpoint, Guid id1, int id2)
            => await HttpGet<T>(endpoint, new[] { id1.ToString(), id2.ToString() });

        public async Task<T> HttpGet<T>(string endpoint, Guid id1, int id2, Guid id3)
            => await HttpGet<T>(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task<T> HttpGet<T>(string endpoint, Guid id1, int id2, string id3)
            => await HttpGet<T>(endpoint, new[] { id1.ToString(), id2.ToString(), id3 });

        public async Task<T> HttpGet<T>(string endpoint, Guid id1, string id2)
            => await HttpGet<T>(endpoint, new[] { id1.ToString(), id2 });

        public async Task<T> HttpGet<T>(string endpoint, Guid id1, string id2, int id3)
            => await HttpGet<T>(endpoint, new[] { id1.ToString(), id2, id3.ToString() });

        public async Task<T> HttpGet<T>(string endpoint, int id)
            => await HttpGet<T>(endpoint, id.ToString());

        public async Task<T> HttpPost<T>(string endpoint, object payload)
        {
            var url = endpoint;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var client = CreateHttpClient();
            var http = await client.PostAsync(url, content);
            var json = await http.Content.ReadAsStringAsync();

            try
            {
                var response = JsonSerializer.Deserialize<T>(json);

                if (http.Headers.TryGetValues(Pagination.HeaderKey, out IEnumerable<string> values))
                    Pagination = JsonSerializer.Deserialize<Pagination>(values.First());

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected JSON deserialization error occurred on the response received from {endpoint}. {ex.Message}. JSON = {json}");
            }
        }

        public async Task HttpPost(string endpoint, object payload)
        {
            var url = endpoint;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var client = CreateHttpClient();
            var http = await client.PostAsync(url, content);
            await http.Content.ReadAsStringAsync();
        }

        public async Task<T> HttpPut<T>(string endpoint, string id, object payload)
        {
            var url = endpoint + "/" + id;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var client = CreateHttpClient();
            var http = await client.PutAsync(url, content);
            var json = await http.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
        }

        public async Task HttpPut(string endpoint, string id, object payload)
        {
            var url = endpoint + "/" + id;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var client = CreateHttpClient();
            var http = await client.PutAsync(url, content);
            await http.Content.ReadAsStringAsync();
        }

        public async Task HttpPut(string endpoint, string[] id, object payload)
        {
            var url = endpoint.ToString();
            if (id != null && id.Length > 0)
                url += "/" + string.Join("/", id);

            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var client = CreateHttpClient();
            var http = await client.PutAsync(url, content);
            await http.Content.ReadAsStringAsync();
        }

        public async Task<T> HttpPut<T>(string endpoint, Guid id, object payload)
            => await HttpPut<T>(endpoint, id.ToString(), payload);

        public async Task HttpPut(string endpoint, int id, object payload)
            => await HttpPut(endpoint, id.ToString(), payload);

        public async Task HttpPut(string endpoint, Guid id, object payload)
            => await HttpPut(endpoint, id.ToString(), payload);

        public async Task HttpPut(string endpoint, Guid id1, Guid id2, object payload)
            => await HttpPut(endpoint, new[] { id1.ToString(), id2.ToString() }, payload);

        public async Task HttpPut(string endpoint, Guid id1, Guid id2, Guid id3, object payload)
            => await HttpPut(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() }, payload);

        public async Task HttpPut(string endpoint, Guid id1, Guid id2, int id3, object payload)
            => await HttpPut(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() }, payload);

        public async Task HttpPut(string endpoint, Guid id1, int id2, object payload)
            => await HttpPut(endpoint, new[] { id1.ToString(), id2.ToString() }, payload);

        public async Task HttpPut(string endpoint, Guid id1, int id2, string id3, object payload)
            => await HttpPut(endpoint, new[] { id1.ToString(), id2.ToString(), id3 }, payload);

        public async Task HttpPut(string endpoint, Guid id1, string id2, object payload)
            => await HttpPut(endpoint, new[] { id1.ToString(), id2 }, payload);

        public async Task HttpPut(string endpoint, Guid id1, string id2, int id3, object payload)
            => await HttpPut(endpoint, new[] { id1.ToString(), id2, id3.ToString() }, payload);

        public async Task HttpDelete(string endpoint, string id)
            => await HttpDelete(endpoint, new[] { id });

        public async Task HttpDelete(string endpoint, string[] id)
        {
            var url = endpoint.ToString();
            if (id != null && id.Length > 0)
                url += "/" + string.Join("/", id);

            var client = CreateHttpClient();
            var http = await client.DeleteAsync(url);
            await http.Content.ReadAsStringAsync();
        }

        public async Task HttpDelete(string endpoint, Guid id)
            => await HttpDelete(endpoint, id.ToString());

        public async Task HttpDelete(string endpoint, Guid id1, Guid id2)
            => await HttpDelete(endpoint, new[] { id1.ToString(), id2.ToString() });

        public async Task HttpDelete(string endpoint, Guid id1, Guid id2, Guid id3)
            => await HttpDelete(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task HttpDelete(string endpoint, Guid id1, Guid id2, int id3)
            => await HttpDelete(endpoint, new[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task HttpDelete(string endpoint, Guid id1, int id2)
            => await HttpDelete(endpoint, new[] { id1.ToString(), id2.ToString() });

        public async Task HttpDelete(string endpoint, Guid id1, int id2, string id3)
            => await HttpDelete(endpoint, new[] { id1.ToString(), id2.ToString(), id3 });

        public async Task HttpDelete(string endpoint, Guid id1, string id2)
            => await HttpDelete(endpoint, new[] { id1.ToString(), id2 });

        public async Task HttpDelete(string endpoint, Guid id1, string id2, int id3)
            => await HttpDelete(endpoint, new[] { id1.ToString(), id2, id3.ToString() });

        public async Task HttpDelete(string endpoint, int id)
            => await HttpDelete(endpoint, id.ToString());

        #endregion

        #region HTTP Request Wrappers (Assert, Count)

        public async Task<bool> Assert(string endpoint, Guid id)
            => await HttpGet<bool>(endpoint, id.ToString());

        public async Task<bool> Assert(string endpoint, Guid id1, Guid id2)
            => await HttpGet<bool>(endpoint, new string[] { id1.ToString(), id2.ToString() });

        public async Task<bool> Assert(string endpoint, Guid id1, Guid id2, Guid id3)
            => await HttpGet<bool>(endpoint, new string[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task<bool> Assert(string endpoint, Guid id1, int id2)
            => await HttpGet<bool>(endpoint, new string[] { id1.ToString(), id2.ToString() });

        public async Task<bool> Assert(string endpoint, Guid id1, int id2, Guid id3)
            => await HttpGet<bool>(endpoint, new string[] { id1.ToString(), id2.ToString(), id3.ToString() });

        public async Task<bool> Assert(string endpoint, Guid id1, int id2, string id3)
            => await HttpGet<bool>(endpoint, new string[] { id1.ToString(), id2.ToString() });

        public async Task<bool> Assert(string endpoint, Guid id1, string id2)
            => await HttpGet<bool>(endpoint, new string[] { id1.ToString(), id2 });

        public async Task<bool> Assert(string endpoint, int id)
            => await HttpGet<bool>(endpoint, id);

        public async Task<bool> Assert(string endpoint, string id)
            => await HttpGet<bool>(endpoint, id);

        public async Task<int> Count(string endpoint, Dictionary<string, string> criteria)
            => await HttpGet<int>(endpoint, criteria);

        #endregion
    }
}