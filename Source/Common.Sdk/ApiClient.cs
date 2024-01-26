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
    public class ApiClient : IDisposable
    {
        private readonly string _apiUrl;
        private HttpClient _client;
        private Pagination _pagination;

        public ApiClient(string url)
        {
            _apiUrl = url;
            _client = new HttpClient();
        }

        public ApiClient(string url, string secret)
        {
            _apiUrl = url;
            _client = new HttpClient();

            var token = GetApiToken(secret);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public string GetApiToken(string secret)
        {
            var provider = new ApiTokenProvider(_client);
            var token = Task.Run(() => provider.GetTokenAsync(_apiUrl, secret))
                .GetAwaiter().GetResult();
            return token;
        }

        public void SetApiToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public T HttpGet<T>(string endpoint, string item)
        {
            return HttpGet<T>(endpoint, new[] { item });
        }

        public T HttpGet<T>(string endpoint, string[] item)
        {
            if (!_apiUrl.EndsWith("/") && !endpoint.StartsWith("/"))
                endpoint = "/" + endpoint;

            var url = _apiUrl + endpoint;
            if (item != null && item.Length > 0)
                url += "/" + string.Join("/", item);

            var http = Task.Run(() => _client.GetAsync(url)).GetAwaiter().GetResult();
            var json = Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
        }

        public IEnumerable<T> HttpGet<T>(string endpoint, Dictionary<string, string> variables)
        {
            if (!_apiUrl.EndsWith("/") && !endpoint.StartsWith("/"))
                endpoint = "/" + endpoint;

            var url = _apiUrl + endpoint + "?";
            foreach (var kvp in variables)
                url += $"{kvp.Key}={kvp.Value}&";

            var http = Task.Run(() => _client.GetAsync(url)).GetAwaiter().GetResult();
            var json = Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
            var response = JsonSerializer.Deserialize<IEnumerable<T>>(json);

            if (http.StatusCode == HttpStatusCode.OK)
            {
                if (http.Headers.TryGetValues(Pagination.HeaderKey, out IEnumerable<string> values))
                {
                    _pagination = JsonSerializer.Deserialize<Pagination>(values.First());
                }
            }

            return response;
        }

        public T HttpPost<T>(string endpoint, object payload)
        {
            if (!_apiUrl.EndsWith("/") && !endpoint.StartsWith("/"))
                endpoint = "/" + endpoint;

            var url = _apiUrl + endpoint;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var http = Task.Run(() => _client.PostAsync(url, content)).GetAwaiter().GetResult();
            var json = Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
            var response = JsonSerializer.Deserialize<T>(json);
            var status = (int)http.StatusCode;
            return response;
        }

        public void HttpPost(string endpoint, object payload)
        {
            if (!_apiUrl.EndsWith("/") && !endpoint.StartsWith("/"))
                endpoint = "/" + endpoint;

            var url = _apiUrl + endpoint;
            var data = JsonSerializer.Serialize(payload);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var http = Task.Run(() => _client.PostAsync(url, content)).GetAwaiter().GetResult();
            Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter();
        }

        public void HttpDelete(string endpoint, string item)
        {
            HttpDelete(endpoint, new[] { item });
        }

        public void HttpDelete(string endpoint, string[] item)
        {
            if (!_apiUrl.EndsWith("/") && !endpoint.StartsWith("/"))
                endpoint = "/" + endpoint;

            var url = _apiUrl + endpoint;
            if (item != null && item.Length > 0)
                url += "/" + string.Join("/", item);

            var http = Task.Run(() => _client.DeleteAsync(url)).GetAwaiter().GetResult();
            Task.Run(() => http.Content.ReadAsStringAsync()).GetAwaiter();
        }

        public static string DictionaryToQueryString(Dictionary<string, string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return string.Empty;
            }

            var queryParams = new List<string>();

            foreach (var kvp in dictionary)
            {
                string key = HttpUtility.UrlEncode(kvp.Key);
                string value = HttpUtility.UrlEncode(kvp.Value);
                queryParams.Add($"{key}={value}");
            }

            return string.Join("&", queryParams);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        public Pagination Pagination
            => _pagination;
    }
}