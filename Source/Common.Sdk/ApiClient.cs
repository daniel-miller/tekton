using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

using Common.Contract;

namespace Common.Sdk
{
    public class ApiClient
    {
        private readonly IApiClientFactory _factory;

        public Pagination Pagination { get; private set; }

        public ApiClient(IApiClientFactory apiClientFactory)
        {
            _factory = apiClientFactory;
        }

        public async Task<T> HttpGet<T>(string endpoint, string item)
            => await HttpGet<T>(endpoint, new[] { item });

        public async Task<T> HttpGet<T>(string endpoint, string[] item)
        {
            var url = endpoint.ToString();
            if (item != null && item.Length > 0)
                url += "/" + string.Join("/", item);

            var client = CreateHttpClient();
            var http = await client.GetAsync(url);
            var json = await http.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
        }

        public async Task<IEnumerable<T>> HttpGet<T>(string endpoint, Dictionary<string, string> variables)
        {
            var url = endpoint.ToString() + "?";
            foreach (var kvp in variables)
                url += $"{kvp.Key}={kvp.Value}&";

            var client = CreateHttpClient();
            var http = await client.GetAsync(url);
            var json = await http.Content.ReadAsStringAsync();

            if (http.StatusCode != HttpStatusCode.OK)
                throw new ApiException($"An unexpected HTTP response was received from {endpoint}: {(int)http.StatusCode} {http.StatusCode}. {http.RequestMessage}");

            try
            {
                var response = JsonSerializer.Deserialize<IEnumerable<T>>(json);

                if (http.Headers.TryGetValues(Pagination.HeaderKey, out IEnumerable<string> values))
                    Pagination = JsonSerializer.Deserialize<Pagination>(values.First());

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An unexpected JSON deserialization error occurred on the response received from {endpoint}. {ex.Message}. JSON = {json}");
            }
        }

        public async Task<T> HttpPost<T>(string endpoint, object payload)
        {
            var url = endpoint;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var client = CreateHttpClient();
            var http = await client.PostAsync(url, content);
            var json = await http.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
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

        public async Task<T> HttpPut<T>(string endpoint, string item, object payload)
        {
            var url = endpoint + "/" + item;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var client = CreateHttpClient();
            var http = await client.PutAsync(url, content);
            var json = await http.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
        }

        public async Task HttpPut(string endpoint, string item, object payload)
        {
            var url = endpoint + "/" + item;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var client = CreateHttpClient();
            var http = await client.PutAsync(url, content);
            await http.Content.ReadAsStringAsync();
        }

        public async Task HttpPut(string endpoint, string[] item, object payload)
        {
            var url = endpoint.ToString();
            if (item != null && item.Length > 0)
                url += "/" + string.Join("/", item);

            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var client = CreateHttpClient();
            var http = await client.PutAsync(url, content);
            await http.Content.ReadAsStringAsync();
        }

        public async Task HttpDelete(string endpoint, string item)
            => await HttpDelete(endpoint, new[] { item });

        public async Task HttpDelete(string endpoint, string[] item)
        {
            var url = endpoint.ToString();
            if (item != null && item.Length > 0)
                url += "/" + string.Join("/", item);

            var client = CreateHttpClient();
            var http = await client.DeleteAsync(url);
            await http.Content.ReadAsStringAsync();
        }

        public static string DictionaryToQueryString(Dictionary<string, string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
                return string.Empty;

            var queryParams = new List<string>();

            foreach (var kvp in dictionary)
            {
                string key = HttpUtility.UrlEncode(kvp.Key);
                string value = HttpUtility.UrlEncode(kvp.Value);
                queryParams.Add($"{key}={value}");
            }

            return string.Join("&", queryParams);
        }

        private HttpClient CreateHttpClient()
        {
            var client = _factory.CreateClient();
            var token = _factory.GetToken();
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}